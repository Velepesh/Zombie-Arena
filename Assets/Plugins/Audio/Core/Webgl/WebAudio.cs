using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Plugins.Audio.Core
{
    public static class WebAudio
    {
        private static Dictionary<string, JsAudioProvider> _sources = new Dictionary<string, JsAudioProvider>();
        
        public static void Init()
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }
            
            InitAudio(OnSourceEndPlay);
        }
        
        public static void RegistrySource(JsAudioProvider source, string id)
        {
            _sources[id] = source;
        }
        
        public static float GetSourceTime(string sourceID)
        {
            return GetAudioSourceTime(sourceID);
        }

        public static void SetSourceTime(string sourceID, float time)
        {
            SetAudioSourceTime(sourceID, time);
        }

        public static void SetVolume(float value)
        {

            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }
            
            SetGlobalVolume(value);
        }
        
        public static void SetSourceVolume(string sourceID, float value)
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }

            value = Mathf.Clamp(value, 0, 1);
            SetAudioSourceVolume(sourceID, value);
        }
    
        public static void SetSourceMute(string sourceID, bool value)
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }

            SetAudioSourceMute(sourceID, value);
        }

        public static void Mute(bool value)
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }

            MuteExtern(value);
        }

        public static void SetSourceLoop(string sourceID, bool loop)
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }

            SetSourceLoopExtern(sourceID, loop);
        }

        public static void StopSource(string sourceID)
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }
            
            StopAudioSource(sourceID);
        }
    
        public static void DeleteSource(string sourceID)
        {
            _sources.Remove(sourceID);
            
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }
        
            DeleteAudioSource(sourceID);
        }

        public static void Pause()
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }

            PauseAudio();
        }

        public static void UnPause()
        {
            if (Application.isEditor || Application.platform != RuntimePlatform.WebGLPlayer)
            {
                return;
            }
            
            UnPauseAudio();
        }

        [DllImport("__Internal")]
        private static extern void InitAudio(Action<string> onSourceEndCallback);

        [DllImport("__Internal")]
        private static extern void SetGlobalVolume(float value);

        [DllImport("__Internal")]
        private static extern void MuteExtern(bool value);

        [DllImport("__Internal")]
        public static extern void PlayAudioSource(string sourceID, string clipPath, bool loop, float volume, bool mute, float pitch, float time);

        [DllImport("__Internal")]
        private static extern void SetSourceLoopExtern(string sourceID, bool loop);

        [DllImport("__Internal")]
        private static extern void SetAudioSourceVolume(string sourceID, float value);

        [DllImport("__Internal")]
        private static extern void SetAudioSourceMute(string sourceID, bool value);

        [DllImport("__Internal")]
        private static extern void StopAudioSource(string sourceID);

        [DllImport("__Internal")]
        private static extern void DeleteAudioSource(string sourceID);

        [DllImport("__Internal")]
        public static extern void SetAudioSourcePitch(string sourceID, float value);
        
        [DllImport("__Internal")]
        public static extern float GetAudioSourcePitch(string sourceID);

        [DllImport("__Internal")]
        private static extern float GetAudioSourceTime(string sourceID);

        [DllImport("__Internal")]
        private static extern void SetAudioSourceTime(string sourceID, float value);
        
        [DllImport("__Internal")]
        public static extern bool IsPlayingAudioSource(string sourceID);
        
        [DllImport("__Internal")]
        public static extern void PauseAudioSource(string sourceID);
        
        [DllImport("__Internal")]
        public static extern void UnpauseAudioSource(string sourceID);
        
        [DllImport("__Internal")]
        private static extern void PauseAudio();
        
        [DllImport("__Internal")]
        private static extern void UnPauseAudio();
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSourceEndPlay(string sourceID)
        {
            if (_sources.TryGetValue(sourceID, out var source))
            {
                source.ClipFinished();
            }
        }
    }
}