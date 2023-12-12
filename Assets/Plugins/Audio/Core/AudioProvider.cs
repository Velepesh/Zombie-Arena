using System;
using UnityEngine.Audio;

namespace Plugins.Audio.Core
{
    public abstract class AudioProvider : IDisposable
    {
        public abstract float SpatialBlend { get; set; }
        public abstract AudioMixerGroup MixerGroup { get; set; }
        public abstract float Volume { get; set; }
        public abstract bool Mute { get; set; }
        public abstract bool Loop { get; set; }
        public abstract float Pitch { get; set; }
        public abstract float Time { get; set; }
        public abstract bool IsPlaying { get; }

        public abstract void Play(string key, float time);
        public abstract void PlayOneShot(string key);
        public abstract void Stop();
        public abstract void Pause();
        public abstract void UnPause();

        public virtual void OnGlobalAudioUnpaused(){}
        public virtual void OnGlobalAudioPaused(){}
        
        public virtual void Update(){}

        public virtual void Dispose(){}

        public virtual void RefreshSettings(SourceAudio.AudioSettings settings)
        {
            Volume = settings.volume;
            Loop = settings.loop;
            Pitch = settings.pitch;
            Mute = settings.mute;
        }
    }
}