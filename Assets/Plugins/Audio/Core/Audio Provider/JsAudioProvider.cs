using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Audio.Core
{
    public class JsAudioProvider : AudioProvider
    {
        public override float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = Mathf.Clamp(value, 0, 1);
                
                WebAudio.SetSourceVolume(_id, _volume);
            }
        }

        public override bool Loop
        {
            get
            {
                return _loop;
            }
            set
            {
                _loop = value;
                
                WebAudio.SetSourceLoop(_id, _loop);
            }
        }

        public override bool Mute
        {
            get
            {
                return _mute;
            }
            set
            {
                _mute = value;
                
                WebAudio.SetSourceMute(_id, _mute);
            }
        }

        public override float SpatialBlend { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override AudioMixerGroup MixerGroup { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override float Pitch
        {
            get
            {
                return WebAudio.GetAudioSourcePitch(_id);
            }
            set
            {
                _pitch = value;
                WebAudio.SetAudioSourcePitch(_id, _pitch);
            }
        }

        public override float Time
        {
            get => WebAudio.GetSourceTime(_id);
            set => WebAudio.SetSourceTime(_id, value);
        }

        private bool _mute;
        private float _volume = 1;
        private bool _loop;
        private float _pitch = 1;

        private readonly string _id;
        
        private readonly SourceAudio _sourceAudio;
        public override bool IsPlaying => WebAudio.IsPlayingAudioSource(_id);

        private bool _isPaused;

        public JsAudioProvider(SourceAudio sourceAudio)
        {
            _sourceAudio = sourceAudio;
            _id = Guid.NewGuid().ToString();

            WebAudio.RegistrySource(this, _id);
        }

        public override void Dispose()
        {
            WebAudio.DeleteSource(_id);
        }


        public override void Play(string key, float time)
        {
            if (string.IsNullOrEmpty(key))
            {
                AudioManagement.Instance.LogError("key is empty, Source Audio PlaySound: " + _sourceAudio.name);
                return;
            }
            
            string clipPath = AudioManagement.Instance.GetClipPath(key);
            _isPaused = false;
                
            WebAudio.PlayAudioSource(_id, clipPath, _loop, _volume, _mute, _pitch, time);

            AudioManagement.Instance.Log("Start play audio: " + key);
        }

        public override void PlayOneShot(string key)
        {
            AudioManagement.Instance.Log("JS Provider not support play one shot");
        }

        public override void Stop()
        {
            WebAudio.StopSource(_id);
        }

        public override void Pause()
        {
            if (_isPaused)
            {
                return;
            }

            _isPaused = true;
            WebAudio.PauseAudioSource(_id);
        }
        
        public override void UnPause()
        {
            if (_isPaused == false)
            {
                return;
            }

            _isPaused = false;
            WebAudio.UnpauseAudioSource(_id);
        }

        public void ClipFinished()
        {
            _sourceAudio.ClipFinished();
        }

        public override void OnGlobalAudioPaused()
        {
            if (_isPaused == false)
            {
                WebAudio.PauseAudioSource(_id);
            }
        }

        public override void OnGlobalAudioUnpaused()
        {
            if (_isPaused == false)
            {
                WebAudio.UnpauseAudioSource(_id);
            }
        }
    }
}