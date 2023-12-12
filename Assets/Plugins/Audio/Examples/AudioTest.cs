using Plugins.Audio.Core;
using Plugins.Audio.Utils;
using UnityEngine;

namespace Plugins.Audio.Examples
{
    public class AudioTest : MonoBehaviour
    {
        [Header("Unity Providers")]
        [SerializeField] private SourceAudio _musicUnitySource;
        [SerializeField] private SourceAudio _soundUnitySource;
        
        [Header("JS Providers")]
        [SerializeField] private SourceAudio _musicJsSource;
        [SerializeField] private SourceAudio _soundJsSource;
        
        [Header("Clips")]
        [SerializeField] private AudioDataProperty _musicClip;
        [SerializeField] private AudioDataProperty _soundClip;

        private SourceAudio _musicSource;
        private SourceAudio _soundSource;

        private void Awake()
        {
            SetAudioProvider(AudioProviderType.Unity);
        }

        public void PlaySound()
        {
            _soundSource.Play(_soundClip);
        }

        public void PlayMusic()
        {
            _musicSource.Play(_musicClip);
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }
        
        public void PauseMusic()
        {
            _musicSource.Pause();
        }

        public void UnPauseMusic()
        {
            _musicSource.UnPause();
        }

        public void SetMusicVolume(float value)
        {
            _musicSource.Volume = value;
        }
        
        public void SetMusicPitch(float value)
        {
            _musicSource.Pitch = value;
        }

        public void SetSoundsVolume(float value)
        {
            _soundSource.Volume = value;
        }
        
        public void SetSoundPitch(float value)
        {
            _soundSource.Pitch = value;
        }

        public void SetAudioProvider(AudioProviderType providerType)
        {
            if (_musicSource != null)
            {
                _musicSource.Stop();
                _soundSource.Stop();
            }
            
            if (providerType == AudioProviderType.Unity)
            {
                _musicSource = _musicUnitySource;
                _soundSource = _soundUnitySource;
            }
            else if(providerType == AudioProviderType.JS)
            {
                _musicSource = _musicJsSource;
                _soundSource = _soundJsSource;
            }
        }
    }
}
