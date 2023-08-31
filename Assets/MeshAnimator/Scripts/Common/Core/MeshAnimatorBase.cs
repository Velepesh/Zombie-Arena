//----------------------------------------------
// Mesh Animator
// Flick Shot Games
// http://www.flickshotgames.com
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;

namespace FSG.MeshAnimator
{
    [RequireComponent(typeof(MeshFilter))]
    public abstract class MeshAnimatorBase : MonoBehaviour, IMeshAnimator
    {
#if UNITY_EDITOR
        public static bool Baking { get; set; }
#endif

        [Serializable]
        public struct MeshAnimatorLODLevel
        {
            public int fps;
            public float distance;
            [NonSerialized]
            public float distanceSquared;
        }
        public Mesh baseMesh;
        public abstract IMeshAnimation defaultAnimation { get; set; }
        public abstract IMeshAnimation[] animations { get; }
        public float speed = 1;
        public bool updateWhenOffscreen = false;
        public bool playAutomatically = true;
        public bool resetOnEnable = true;
        public GameObject eventReciever;
        public int FPS = 30;
        public bool skipLastLoopFrame = false;
        public Action<string> OnAnimationFinished { get; set; }
        public Action OnFrameUpdated { get; set; }
        public Action<bool> OnVisibilityChanged { get; set; }
        public int currentFrame;
        public Transform LODCamera;
        public MeshAnimatorLODLevel[] LODLevels = new MeshAnimatorLODLevel[0];

        [HideInInspector]
        public float nextTick = 0;
        [HideInInspector]
        public MeshFilter meshFilter;
        [HideInInspector]
        public MeshRenderer meshRenderer;
        [HideInInspector]
        public MeshAnimationBase currentAnimation;

        protected int currentAnimIndex = -1;
        protected bool isVisible = true;
        protected float lastFrameTime;
        protected bool isPaused = false;
        protected float currentAnimTime;
        protected Queue<string> queuedAnims;
        protected int currentLodLevel = 0;
        protected Transform mTransform;
        protected Dictionary<string, Transform> childMap;
        protected bool initialized = false;
        protected int previousEventFrame = -1;
        protected bool hasExposedTransforms;
        protected bool hasLODCamera;
        protected float nextLODCheck = 0;
        protected int animationCount;
        protected ExposedTransformCrossfade exposedTransformCrossfade;

        public MeshRenderer MeshRenderer { get { return meshRenderer; } }
        public MeshFilter MeshFilter { get { return meshFilter; } }
        public IMeshAnimation[] Animations { get { return animations; } }
        public float NextTick { get { return nextTick; } }

        #region Private Methods
        protected virtual void Start()
        {
            var animations = this.animations;
            if (animations == null || animations.Length == 0)
            {
                Debug.LogWarning("No animations for MeshAnimator on object: " + name + ". Disabling.", this);
                this.enabled = false;
                animationCount = 0;
                return;
            }
            animationCount = animations.Length;
            for (int i = 0; i < animationCount; i++)
            {
                IMeshAnimation animation = animations[i];
                if (animation == null)
                    continue;
                animation.GenerateFrames(baseMesh);
                var exposedTransforms = animation.ExposedTransforms;
                if (exposedTransforms != null)
                {
                    for (int j = 0; j < exposedTransforms.Length; j++)
                    {
                        string childName = exposedTransforms[j];
                        if (string.IsNullOrEmpty(childName))
                            continue;
                        Transform childTransform = transform.Find(childName);
                        if (childTransform != null)
                        {
                            if (childMap == null)
                            {
                                childMap = new Dictionary<string, Transform>();
                            }
                            if (childMap.ContainsKey(childName) == false)
                            {
                                childMap.Add(childName, childTransform);
                                hasExposedTransforms = true;
                            }
                        }
                    }
                }
            }

            if (meshFilter == null)
                meshFilter = GetComponent<MeshFilter>();

            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();

            if (playAutomatically)
                Play(defaultAnimation.AnimationName);
            else
                isPaused = true;

            for (int i = 0; i < LODLevels.Length; i++)
            {
                float d = LODLevels[i].distance;
                LODLevels[i].distanceSquared = d * d;
            }
            initialized = true;
        }
        private void OnBecameVisible()
        {
            isVisible = true;
            if (OnVisibilityChanged != null)
                OnVisibilityChanged(isVisible);
        }
        private void OnBecameInvisible()
        {
            isVisible = false;
            if (OnVisibilityChanged != null)
                OnVisibilityChanged(isVisible);
        }
        protected virtual void OnEnable()
        {
            mTransform = transform;
            if (resetOnEnable && meshFilter)
            {
                if (playAutomatically)
                {
                    Play(defaultAnimation.AnimationName);
                }
                else
                {
                    isPaused = true;
                }
                if (currentAnimation != null)
                {
                    currentAnimation.GenerateFrame(baseMesh, currentFrame);
                    currentAnimation.DisplayFrame(this, currentFrame, -1);
                }
            }
            if (Application.isPlaying)
                MeshAnimatorManager.AddAnimator(this);
            lastFrameTime = Time.time;
        }
        protected virtual void OnDisable()
        {
            if (Application.isPlaying)
                MeshAnimatorManager.RemoveAnimator(this);
            currentAnimIndex = -1;
            if (queuedAnims != null)
                queuedAnims.Clear();
        }
        protected virtual void OnDestroy()
        {
            ReturnCrossfadeToPool(true);
        }
        protected virtual void AddMeshCount(Dictionary<Mesh, int> counter)
        {
            if (baseMesh == null)
                return;
            if (counter.ContainsKey(baseMesh))
                counter[baseMesh]++;
            else
                counter.Add(baseMesh, 1);
        }
        protected virtual void RemoveMeshCount(Dictionary<Mesh, int> counter)
        {
            if (!counter.ContainsKey(baseMesh))
                return;
            counter[baseMesh]--;
            if (counter[baseMesh] <= 0)
            {
                counter.Remove(baseMesh);
                for (int i = 0; i < animations.Length; i++)
                {
                    animations[i].Reset();
                }
            }
        }
        private void FireAnimationEvents(IMeshAnimation meshAnimation, float totalSpeed, bool finished)
        {
            MeshAnimationBase baseAnimation = meshAnimation as MeshAnimationBase;
            if (baseAnimation.events.Length > 0 && eventReciever != null && previousEventFrame != currentFrame)
            {
                int direction = totalSpeed > 0 ? 1 : -1;
                int targetFrame = currentFrame;
                int totalFrames = meshAnimation.TotalFrames;
                while (previousEventFrame != targetFrame)
                {
                    previousEventFrame += direction;
                    if (previousEventFrame < 0) previousEventFrame = totalFrames;
                    else if (previousEventFrame > totalFrames) previousEventFrame = 0;
                    meshAnimation.FireEvents(eventReciever, previousEventFrame);
                }
            }
        }
        private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
        {
            Matrix4x4 ret = new Matrix4x4();
            for (int i = 0; i < 16; i++)
                ret[i] = Mathf.Lerp(from[i], to[i], time);
            return ret;
        }
        protected virtual bool UpdateExposedTransformCrossfade(int previousFrame)
        {
            if (currentFrame == previousFrame || currentAnimIndex < 0)
                return false;
            if (hasExposedTransforms && exposedTransformCrossfade.isFading)
            {
                if (exposedTransformCrossfade.currentFrame >= exposedTransformCrossfade.framesNeeded)
                {
                    exposedTransformCrossfade.ReturnFrame(false);
                }
                else
                {
                    exposedTransformCrossfade.exposedTransformJobHandles[exposedTransformCrossfade.currentFrame].Complete();
                    if (exposedTransformCrossfade.outputMatrix != null)
                    {
                        var exposedTransforms = currentAnimation.ExposedTransforms;
                        Matrix4x4[] positions = AllocatedArray<Matrix4x4>.Get(exposedTransforms.Length);
                        exposedTransformCrossfade.outputMatrix[exposedTransformCrossfade.currentFrame].CopyTo(positions);
                        for (int i = 0; i < positions.Length; i++)
                        {
                            Transform child = null;
                            if (childMap.TryGetValue(exposedTransforms[i], out child))
                            {
                                MatrixUtils.FromMatrix4x4(child, positions[i]);
                            }
                        }
                    }
                    // apply root motion
                    var fromFrame = exposedTransformCrossfade.fromFrame;
                    var toFrame = exposedTransformCrossfade.toFrame;
                    bool applyRootMotion = currentAnimation.RootMotionMode == RootMotionMode.AppliedToTransform;
                    if (applyRootMotion)
                    {
                        float delta = exposedTransformCrossfade.currentFrame / (float)exposedTransformCrossfade.framesNeeded;
                        Vector3 pos = Vector3.Lerp(fromFrame.rootMotionPosition, toFrame.rootMotionPosition, delta);
                        Quaternion rot = Quaternion.Lerp(fromFrame.rootMotionRotation, toFrame.rootMotionRotation, delta);
                        transform.Translate(pos, Space.Self);
                        transform.Rotate(rot.eulerAngles * Time.deltaTime, Space.Self);
                    }
                    exposedTransformCrossfade.currentFrame++;
                    return false;
                }
            }
            return true;
        }
        protected virtual bool DisplayFrame(int previousFrame)
        {
            if (currentFrame == previousFrame || currentAnimIndex < 0)
                return false;
            if (!UpdateExposedTransformCrossfade(previousFrame)) return false;
            // generate frames if needed and show the current animation frame
            currentAnimation.GenerateFrame(baseMesh, currentFrame);
            currentAnimation.DisplayFrame(this, currentFrame, previousFrame);
            return true;
        }
        protected virtual void OnAnimationCompleted(bool stopUpdate) { }
        protected virtual bool StartCrossfade(int index, float speed)
        {
            if (index < 0 || animations.Length <= index || currentAnimIndex == index)
                return false;
            exposedTransformCrossfade.Reset(false);
            exposedTransformCrossfade.framesNeeded = (int)(speed * FPS);
            exposedTransformCrossfade.isFading = true;
            exposedTransformCrossfade.endTime = Time.time + speed;
            if (currentAnimation == null)
            {
                exposedTransformCrossfade.fromFrame = defaultAnimation.GetNearestFrame(0);
            }
            else
            {
                exposedTransformCrossfade.fromFrame = currentAnimation.GetNearestFrame(currentFrame);
            }
            Play(index);
            exposedTransformCrossfade.toFrame = currentAnimation.GetNearestFrame(0);
            exposedTransformCrossfade.StartCrossfade(exposedTransformCrossfade.fromFrame, exposedTransformCrossfade.toFrame);
            return true;
        }
        protected virtual void ReturnCrossfadeToPool(bool destroying)
        {
            exposedTransformCrossfade.Reset(destroying);
        }
        protected abstract void OnCurrentAnimationChanged(IMeshAnimation meshAnimation);
        protected void GetAnimationStartData(MeshAnimationBase meshAnimationBase, out int frame, out float time)
        {
            if (speed * meshAnimationBase.playbackSpeed < 0 && !meshAnimationBase.IsLoopingWrapMode)
            {
                frame = meshAnimationBase.TotalFrames;
                time = meshAnimationBase.Length;
            }
            else
            {
                frame = 0;
                time = 0;
            }
        }

        #endregion

        #region Public Methods
        public abstract void SetAnimations(IMeshAnimation[] meshAnimations);
        public abstract void StoreAdditionalMeshData(Mesh mesh);

        /// <summary>
        /// The main update loop called from the MeshAnimatorManager
        /// </summary>
        /// <param name="time">Current time</param>
        public void UpdateTick(float time)
        {
            if (initialized == false)
                return;
            if (animationCount == 0)
                return;
            if (currentAnimIndex < 0 || currentAnimIndex > animationCount)
            {
                if (defaultAnimation != null)
                    Play(defaultAnimation.AnimationName);
                else
                    Play(0);
            }
            if ((isVisible == false && updateWhenOffscreen == false) || isPaused || speed == 0 || currentAnimation.playbackSpeed == 0) // return if offscreen or crossfading
            {
                return;
            }
            // update the lod level if needed
            if (LODLevels.Length > 0 && time > nextLODCheck)
            {
                if (!hasLODCamera)
                {
                    int cameraCount = Camera.allCamerasCount;
                    if (cameraCount > 0)
                    {
                        Camera[] cameras = AllocatedArray<Camera>.Get(cameraCount);
                        cameraCount = Camera.GetAllCameras(cameras);
                        LODCamera = cameras[0].transform;
                        AllocatedArray<Camera>.Return(cameras);
                        hasLODCamera = true;
                    }
                }
                if (hasLODCamera)
                {
                    float dis = (LODCamera.position - mTransform.position).sqrMagnitude;
                    int lodLevel = 0;
                    for (int i = 0; i < LODLevels.Length; i++)
                    {
                        if (dis > LODLevels[i].distanceSquared)
                        {
                            lodLevel = i;
                        }
                    }
                    if (currentLodLevel != lodLevel)
                    {
                        currentLodLevel = lodLevel;
                    }
                }
                nextLODCheck = time + UnityEngine.Random.Range(0.5f, 1.5f);
            }
            // if the speed is below the normal playback speed, wait until the next frame can display
            float lodFPS = LODLevels.Length > currentLodLevel ? LODLevels[currentLodLevel].fps : FPS;
            if (lodFPS == 0.0f)
            {
                return;
            }

            float directionalSpeed = currentAnimation.playbackSpeed * speed;
            float totalSpeed = Math.Abs(directionalSpeed);
            float calculatedTick = 1f / lodFPS / totalSpeed;
            float tickRate = 0.0001f;
            if (calculatedTick > 0.0001f)
                tickRate = calculatedTick;
            float actualDelta = time - lastFrameTime;
            bool finished = false;

            currentAnimTime += actualDelta * directionalSpeed;

            bool pingPonging = false;
            if (currentAnimation.IsLoopingWrapMode)
            {
                if (currentAnimTime < 0f || currentAnimTime > currentAnimation.Length)
                    finished = true;
                if (currentAnimation.wrapMode == WrapMode.PingPong)
                    pingPonging = Mathf.Floor(currentAnimTime / currentAnimation.Length) % 2 == 0;
                else
                    currentAnimTime %= currentAnimation.length;
            }
            else if (currentAnimTime < 0f || currentAnimTime > currentAnimation.Length)
            {
                currentAnimTime = directionalSpeed < 0 ? 0f : currentAnimation.length;
                finished = true;
            }

            nextTick = time + tickRate;
            lastFrameTime = time;

            float normalizedTime = currentAnimation.wrapMode != WrapMode.PingPong ? 
                Mathf.Clamp01(currentAnimTime / currentAnimation.length) : 
                Mathf.Clamp01((currentAnimTime % currentAnimation.length) / currentAnimation.length);
            if (pingPonging) normalizedTime = 1.0f - normalizedTime;
            int previousFrame = currentFrame;
            int totalFrames = currentAnimation.TotalFrames;
            currentFrame = (int)Math.Round(normalizedTime * totalFrames, MidpointRounding.AwayFromZero);

            if (finished)
            {
                bool stopUpdate = false;
                if (queuedAnims != null && queuedAnims.Count > 0)
                {
                    Play(queuedAnims.Dequeue());
                    stopUpdate = true;
                }
                else if (!currentAnimation.IsLoopingWrapMode)
                {
                    nextTick = float.MaxValue;
                    stopUpdate = true;
                    isPaused = true;
                }
                if (OnAnimationFinished != null)
                    OnAnimationFinished(currentAnimation.AnimationName);
                OnAnimationCompleted(stopUpdate);
                if (stopUpdate)
                {
                    FireAnimationEvents(currentAnimation, directionalSpeed * (pingPonging ? -1f : 1f), true);
                    return;
                }
            }

            bool updateFrameTransforms = DisplayFrame(previousFrame);
            if (updateFrameTransforms && currentFrame != previousFrame)
            {
                bool applyRootMotion = currentAnimation.rootMotionMode == RootMotionMode.AppliedToTransform;
                if (hasExposedTransforms || applyRootMotion)
                {
                    MeshFrameDataBase fromFrame = currentAnimation.GetNearestFrame(currentFrame);
                    MeshFrameDataBase targetFrame = null;
                    int frameGap = currentFrame % currentAnimation.FrameSkip;
                    bool needsInterp = actualDelta > 0 && frameGap != 0;
                    float blendDelta = 0;
                    if (needsInterp)
                    {
                        blendDelta = currentAnimation.GetInterpolatingFrames(currentFrame, out fromFrame, out targetFrame);
                    }
                    // move exposed transforms
                    if (hasExposedTransforms)
                    {
                        var exposedTransforms = currentAnimation.ExposedTransforms;
                        for (int i = 0; i < exposedTransforms.Length; i++)
                        {
                            Transform child = null;
                            if (fromFrame.exposedTransforms.Length > i && childMap.TryGetValue(exposedTransforms[i], out child))
                            {
                                if (needsInterp)
                                {
                                    Matrix4x4 c = MatrixLerp(fromFrame.exposedTransforms[i], targetFrame.exposedTransforms[i], blendDelta);
                                    MatrixUtils.FromMatrix4x4(child, c);
                                }
                                else
                                {
                                    MatrixUtils.FromMatrix4x4(child, fromFrame.exposedTransforms[i]);
                                }
                            }
                        }
                    }
                    // apply root motion
                    if (applyRootMotion)
                    {
                        if (previousFrame > currentFrame)
                        {
                            // animation looped around, apply motion for skipped frames at the end of the animation
                            for (int i = previousFrame + 1; i < currentAnimation.Frames.Length; i++)
                            {
                                MeshFrameDataBase rootFrame = currentAnimation.GetNearestFrame(i);
                                transform.Translate(rootFrame.rootMotionPosition, Space.Self);
                                transform.Rotate(rootFrame.rootMotionRotation.eulerAngles * actualDelta, Space.Self);
                            }
                            // now apply motion from first frame to current frame
                            for (int i = 0; i <= currentFrame; i++)
                            {
                                MeshFrameDataBase rootFrame = currentAnimation.GetNearestFrame(i);
                                transform.Translate(rootFrame.rootMotionPosition, Space.Self);
                                transform.Rotate(rootFrame.rootMotionRotation.eulerAngles * actualDelta, Space.Self);
                            }
                        }
                        else
                        {
                            for (int i = previousFrame + 1; i <= currentFrame; i++)
                            {
                                MeshFrameDataBase rootFrame = currentAnimation.GetNearestFrame(i);
                                transform.Translate(rootFrame.rootMotionPosition, Space.Self);
                                transform.Rotate(rootFrame.rootMotionRotation.eulerAngles * actualDelta, Space.Self);
                            }
                        }
                    }
                }
            }
            if (OnFrameUpdated != null)
                OnFrameUpdated();

            FireAnimationEvents(currentAnimation, directionalSpeed * (pingPonging ? -1f : 1f), finished);
        }

        /// <summary>
        /// Play the default animation, or resume playing a paused animator
        /// </summary>
        public virtual void Play()
        {
            isPaused = false;
            nextTick = 0;
            isPaused = false;
            lastFrameTime = Time.time;
        }

        /// <summary>
        /// Play an animation by name
        /// </summary>
        /// <param name="animationName">Name of the animation</param>
        public virtual void Play(string animationName, float normalizedTime = -1)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].IsName(animationName))
                {
                    Play(i);
                    break;
                }
            }
            if (normalizedTime != -1)
            {
                SetTimeNormalized(Mathf.Clamp01(normalizedTime));
            }
        }

        /// <summary>
        /// Play an animation by index
        /// </summary>
        /// <param name="index">Index of the animation</param>
        public virtual void Play(int index)
        {
            if (index < 0 || animations.Length <= index || currentAnimIndex == index)
                return;
            if (queuedAnims != null)
                queuedAnims.Clear();
            currentAnimIndex = index;
            currentAnimation = animations[currentAnimIndex] as MeshAnimationBase;
            GetAnimationStartData(currentAnimation, out currentFrame, out currentAnimTime);
            previousEventFrame = -1;
            isPaused = false;
            nextTick = Time.time;
            OnCurrentAnimationChanged(currentAnimation);
        }

        /// <summary>
        /// Play a random animation
        /// </summary>
        /// <param name="animationNames">Animation names</param>
        public void PlayRandom(params string[] animationNames)
        {
            int rand = UnityEngine.Random.Range(0, animationNames.Length);
            string randomAnim = animationNames[rand];
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].IsName(randomAnim))
                {
                    Play(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Play an animation after the previous one has finished
        /// </summary>
        /// <param name="animationName">Animation name</param>
        public void PlayQueued(string animationName)
        {
            if (queuedAnims == null)
                queuedAnims = new Queue<string>();
            queuedAnims.Enqueue(animationName);
        }

        /// <summary>
        /// Pause an animator, disabling the component also has the same effect
        /// </summary>
        public virtual void Pause()
        {
            isPaused = true;
        }

        /// <summary>
        /// Restart the current animation from the beginning
        /// </summary>
        public virtual void RestartAnim()
        {
            GetAnimationStartData(currentAnimation, out currentFrame, out currentAnimTime);
            nextTick = 0;
            isPaused = false;
            lastFrameTime = Time.time;
        }

        /// <summary>
        /// Crossfade an animation by index
        /// </summary>
        /// <param name="index">Index of the animation</param>
        public void Crossfade(int index)
        {
            Crossfade(index, 0.1f);
        }

        /// <summary>
        /// Crossfade an animation by name
        /// </summary>
        /// <param name="animationName">Name of the animation</param>
        public void Crossfade(string animationName)
        {
            Crossfade(animationName, 0.1f);
        }

        /// <summary>
        /// Crossfade an animation by index
        /// </summary>
        /// <param name="index">Index of the animation</param>
        /// <param name="speed">Duration the crossfade will take</param>
        public abstract void Crossfade(int index, float speed);

        /// <summary>
        /// Crossfade an animation by name
        /// </summary>
        /// <param name="animationName">Name of the animation</param>
        /// <param name="speed">Duration the crossfade will take</param>
        public void Crossfade(string animationName, float speed)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].IsName(animationName))
                {
                    Crossfade(i, speed);
                    break;
                }
            }
        }

        /// <summary>
        /// Populates the crossfade pool with the set amount of meshes
        /// </summary>
        /// <param name="count">Amount to fill the pool with</param>
        public abstract void PrepopulateCrossfadePool(int count);

        /// <summary>
        /// Sets the current time of the playing animation
        /// </summary>
        /// <param name="time">Time of the animation to play. Min: 0, Max: Length of animation</param>
        public virtual void SetTime(float time, bool instantUpdate = false)
        {
            var cAnim = currentAnimation;
            if (cAnim == null)
                return;
            time = Mathf.Clamp(time, 0, cAnim.Length);
            currentAnimTime = time;
            if (isPaused)
                Play();
            if (instantUpdate)
                UpdateTick(Time.time);
        }

        /// <summary>
        /// Set the current time of the animation, normalized
        /// </summary>
        /// <param name="time">Time of the animation to start playback (0-1)</param>
        public virtual void SetTimeNormalized(float time, bool instantUpdate = false)
        {
            var cAnim = currentAnimation;
            if (cAnim == null)
                return;
            time = Mathf.Clamp01(time);
            currentAnimTime = time * cAnim.Length;
            if (isPaused)
                Play();
            if (instantUpdate)
                UpdateTick(Time.time);
        }

        /// <summary>
        /// Get the MeshAnimation by name
        /// </summary>
        /// <param name="animationName">Name of the animation</param>
        /// <returns>MeshAnimation class</returns>
        public IMeshAnimation GetClip(string animationName)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i].IsName(animationName))
                {
                    return animations[i];
                }
            }
            return null;
        }

        public void DisplayFrame(int frame, MeshRenderer meshRenderer)
        {
            throw new NotImplementedException();
        }

        public bool IsPlaying()
        {
            return !isPaused;
        }
        #endregion
    }
}