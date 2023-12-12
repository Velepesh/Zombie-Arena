using System.Collections;
using Plugins.Audio.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Audio.Core
{
    public class StreamingAudioProvider : BaseUnityAudioProvider
    {
        public override float SpatialBlend
        {
            get => _unitySource.spatialBlend;
            set => _unitySource.spatialBlend = value;
        }

        public override AudioMixerGroup MixerGroup
        {
            get => _unitySource.outputAudioMixerGroup;
            set => _unitySource.outputAudioMixerGroup = value;
        }

        public override float Volume
        {
            get => _unitySource.volume;
            set => _unitySource.volume = value;
        }
        
        public override bool Mute
        {
            get => _unitySource.mute;
            set => _unitySource.mute = value;
        }

        public override bool Loop { get => _unitySource.loop; set => _unitySource.loop = value; }

        public override float Pitch
        {
            get => _unitySource.pitch;
            set => _unitySource.pitch = value;
        }

        public override float Time
        {
            get => _unitySource.time;
            //set => _unitySource.time = value;
            set => _unitySource.timeSamples = TimeToSamplesTime(value);
        }

        public override bool IsPlaying => _unitySource.isPlaying;

        private AudioSource _unitySource;
        
        private Coroutine _playRoutine;
        
        private AudioClip _clip;

        private bool _isFocus = true;
        private bool _loadClip;

        private bool _beginPlaying = false;
        private int _lastTimeSamples;
        private SourceAudio _sourceAudio;

        private string _key;
        private bool _isPaused;
        
        public StreamingAudioProvider(SourceAudio sourceAudio)
        {
            _sourceAudio = sourceAudio;
            _unitySource = CreateAudioSource(sourceAudio);

            _unitySource.ignoreListenerPause = true;
        }
        
        public override void RefreshSettings(SourceAudio.AudioSettings settings)
        {
            base.RefreshSettings(settings);

            _unitySource.SetData(settings);
        }

        public override void Play(string key, float time)
        {
            if (string.IsNullOrEmpty(key))
            {
                AudioManagement.Instance.LogError("key is empty, Source Audio PlaySound: " + _sourceAudio.name);
                return;
            }

            if (_playRoutine != null)
            {
                _sourceAudio.StopCoroutine(_playRoutine);
            }
            
            _playRoutine = _sourceAudio.StartCoroutine(PlayRoutine(key, time));
        }

        public override void PlayOneShot(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                AudioManagement.Instance.LogError("key is empty, Source Audio PlaySound: " + _sourceAudio.name);
                return;
            }

            _playRoutine = _sourceAudio.StartCoroutine(PlayOneShotRoutine(key));
        }

        private IEnumerator PlayRoutine(string key, float time)
        {
            _loadClip = true;
            _clip = null;
            _key = key;
            
            _unitySource.Stop();
            _beginPlaying = false;
            
            yield return AudioManagement.Instance.GetClip(key, audioClip => _clip = audioClip);

            if (_clip == null)
            {
                AudioManagement.Instance.LogError("Audio Management not found clip at key: " + key + ",\n Source Audio PlaySound: " + _sourceAudio.name);
                yield break;
            }

            _isPaused = false;
            _unitySource.clip = _clip;
            _unitySource.Play();
            _beginPlaying = true;
            _lastTimeSamples = TimeToSamplesTime(time);

            _unitySource.timeSamples = TimeToSamplesTime(time);
            _loadClip = false;
            
            AudioManagement.Instance.Log("Start play audio: " + key);
        }

        private int TimeToSamplesTime(float time)
        {
            int sampleRate = AudioSettings.outputSampleRate;
            
            int samplesTime = Mathf.RoundToInt(time * sampleRate);

            return samplesTime;
        }

        private IEnumerator PlayOneShotRoutine(string key)
        {
            AudioClip clip = null;
            
            yield return AudioManagement.Instance.GetClip(key, audioClip => clip = audioClip);

            if (clip == null)
            {
                AudioManagement.Instance.LogError("Audio Management not found clip at key: " + key + ",\n Source Audio PlaySound: " + _sourceAudio.name);
                yield break;
            }

            _unitySource.PlayOneShot(clip);
            
            AudioManagement.Instance.Log("Start play audio: " + key);
        }

        public override void Stop()
        {
            _unitySource.Stop();
            _beginPlaying = false;
            Time = 0;
            _lastTimeSamples = 0;
        }

        public override void Pause()
        {
            if (_isPaused)
            {
                return;
            }
            
            _isPaused = true;
            _lastTimeSamples = _unitySource.timeSamples;
            _unitySource.Pause();
        }

        public override void UnPause()
        {
            if (_isPaused == false)
            {
                return;
            }
            
            _isPaused = false;

            _unitySource.UnPause();
            _unitySource.timeSamples = _lastTimeSamples;
        }

        public override void Update()
        {
            CheckFinished();
        }
        
        private void CheckFinished()
        {
            if (_clip == null || _loadClip)
            {
                return;
            }
            
            if (_unitySource.time <= 0 && _unitySource.isPlaying == false && _beginPlaying)
            {
                _beginPlaying = false;
                _sourceAudio.ClipFinished();
            }
        }

        public override void OnGlobalAudioUnpaused()
        {
            if (_isFocus == false && _beginPlaying && _lastTimeSamples > 0 && _isPaused == false)
            {
                _unitySource.UnPause();
                _unitySource.timeSamples = _lastTimeSamples;
                AudioManagement.Instance.Log(_sourceAudio.CurrentKey + " Play Last Time: " + _lastTimeSamples);
            }
            
            _isFocus = true;
        }

        public override void OnGlobalAudioPaused()
        {
            if (_isFocus && _beginPlaying && _isPaused == false)
            {
                _lastTimeSamples = _unitySource.timeSamples;
                _unitySource.Pause();
                AudioManagement.Instance.Log(_sourceAudio.CurrentKey + "Set Last Time: " + _lastTimeSamples);
            }
            
            _isFocus = false;
        }
    }
}