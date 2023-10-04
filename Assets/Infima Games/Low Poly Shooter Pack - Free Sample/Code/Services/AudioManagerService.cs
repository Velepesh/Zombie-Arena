using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static Unity.VisualScripting.Member;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Manages the spawning and playing of sounds.
    /// </summary>
    public class AudioManagerService : MonoBehaviour, IAudioManagerService
    {
        /// <summary>
        /// Contains data related to playing a OneShot audio.
        /// </summary>
        private readonly struct OneShotCoroutine
        {
            /// <summary>
            /// Audio Clip.
            /// </summary>
            public AudioClip Clip { get; }
            /// <summary>
            /// Audio VolumeSettings.
            /// </summary>
            public AudioSettings Settings { get; }
            /// <summary>
            /// DelayBeforeMove.
            /// </summary>
            public float Delay { get; }

            /// <summary>
            /// Constructor.
            /// </summary>
            public OneShotCoroutine(AudioClip clip, AudioSettings settings, float delay)
            {
                //Clip.
                Clip = clip;
                //VolumeSettings
                Settings = settings;
                //DelayBeforeMove.
                Delay = delay;
            }
        }

        /// <summary>
        /// Destroys the audio source once it has finished playing.
        /// </summary>
        private IEnumerator DestroySourceWhenFinished(AudioSource source)
        {
            //WaitBeforeLockCursor for the audio source to complete playing the clip.
            yield return new WaitWhile(() => IsPlaying(source));

            //Destroy the audio game object, since we're not using it anymore.
            //This isn't really too great for performance, but it works, for now.
            if (source.IsDestroyed() == false)
                DestroyImmediate(source.gameObject);
        }
        private bool IsPlaying(AudioSource source)
        {
            if (source.IsDestroyed())
                return false;

            return source.isPlaying;
        }
        /// <summary>
        /// Waits for a certain amount of time before starting to play a one shot sound.
        /// </summary>
        private IEnumerator PlayOneShotAfterDelay(OneShotCoroutine value)
        {
            //WaitBeforeLockCursor for the delay.
            yield return new WaitForSeconds(value.Delay);
            //PlayOneShot.
            PlayOneShot_Internal(value.Clip, value.Settings);
        }

        /// <summary>
        /// Internal PlayOneShot. Basically does the whole function's name!
        /// </summary>
        private void PlayOneShot_Internal(AudioClip clip, AudioSettings settings)
        {
            //No need to do absolutely anything if the clip is null.
            if (clip == null)
                return;

            //Spawn a game object for the audio source.
            var newSourceObject = new GameObject($"Audio Source -> {clip.name}");
            //Add an audio source component to that object.
            var newAudioSource = newSourceObject.AddComponent<AudioSource>();

            //Set volume.
            newAudioSource.volume = settings.Volume;
            //Set spatial blend.
            newAudioSource.spatialBlend = settings.SpatialBlend;
            newAudioSource.outputAudioMixerGroup = settings.MixerGroup;
           // Debug.Log(settings.MixerGroup.name + " !");
            //PlayOneShot the clip!
            newAudioSource.PlayOneShot(clip);

            //Start a coroutine that will destroy the whole object once it is done!
            if (settings.AutomaticCleanup)
                StartCoroutine(nameof(DestroySourceWhenFinished), newAudioSource);
        }

        #region Audio Manager Service Interface

        public void PlayOneShot(AudioClip clip, AudioSettings settings = default)
        {
            //PlayOneShot.
            PlayOneShot_Internal(clip, settings);
        }

        public void PlayOneShotDelayed(AudioClip clip, AudioSettings settings = default, float delay = 1.0f)
        {
            //PlayOneShot.
            StartCoroutine(nameof(PlayOneShotAfterDelay), new OneShotCoroutine(clip, settings, delay));
        }

        #endregion
    }
}
