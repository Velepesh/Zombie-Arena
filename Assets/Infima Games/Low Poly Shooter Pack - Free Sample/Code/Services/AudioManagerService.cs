using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Plugins.Audio.Utils;
using Plugins.Audio.Core;

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
            public AudioDataProperty Clip { get; }
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
            public OneShotCoroutine(AudioDataProperty clip, AudioSettings settings, float delay)
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
        private IEnumerator DestroySourceWhenFinished(SourceAudio source)
        {
            //WaitBeforeLockCursor for the audio source to complete playing the clip.
            yield return new WaitWhile(() => IsPlaying(source));
            //Destroy the audio game object, since we're not using it anymore.
            //This isn't really too great for performance, but it works, for now.
            if (source != null)
                Destroy(source.gameObject, 5f);
        }
        private bool IsPlaying(SourceAudio source)
        {
            if (source.IsDestroyed())
                return false;

            return source.IsPlaying;
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
        private void PlayOneShot_Internal(AudioDataProperty clip, AudioSettings settings)
        {
            //No need to do absolutely anything if the clip is null.
            if (clip == null)
                return;

            //Spawn a game object for the audio source.
            var newSourceObject = new GameObject($"Audio Source -> {clip.Key}");
            //Add an audio source component to that object.
            var newAudioSource = newSourceObject.AddComponent<SourceAudio>();

            //Set volume.
            newAudioSource.Volume = settings.Volume;
            //Set spatial blend.
            newAudioSource.SpatialBlend = settings.SpatialBlend;
            newAudioSource.MixerGroup = settings.MixerGroup;
            //PlayOneShot the clip!
            newAudioSource.PlayOneShot(clip.Key);

            //Start a coroutine that will destroy the whole object once it is done!
            if (settings.AutomaticCleanup)
                StartCoroutine(nameof(DestroySourceWhenFinished), newAudioSource);
        }

        #region Audio Manager Service Interface

        public void PlayOneShot(AudioDataProperty clip, AudioSettings settings = default)
        {
            //PlayOneShot.
            PlayOneShot_Internal(clip, settings);
        }

        public void PlayOneShotDelayed(AudioDataProperty clip, AudioSettings settings = default, float delay = 1.0f)
        {
            //PlayOneShot.
            StartCoroutine(nameof(PlayOneShotAfterDelay), new OneShotCoroutine(clip, settings, delay));
        }

        #endregion
    }
}
