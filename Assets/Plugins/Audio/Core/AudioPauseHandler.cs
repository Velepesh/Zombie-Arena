using System;
using UnityEngine;

namespace Plugins.Audio.Core
{
    public class AudioPauseHandler : MonoBehaviour
    {
        public static AudioPauseHandler Instance => _instance;

        private static AudioPauseHandler _instance;

        public static event Action OnPause;
        public static event Action OnUnpause;

        public static bool IsAudioPause { get; private set; }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            IsAudioPause = false;
            DontDestroyOnLoad(gameObject);
        }

        public void Pause()
        {
            if (IsAudioPause == true)
                return;

            AudioListener.pause = true;
            WebAudio.Mute(true);

            IsAudioPause = true;
            OnPause?.Invoke();
            AudioManagement.Instance.Log("Pause Audio");
        }

        public void Unpause()
        {
            if (IsAudioPause == false)
                return;

            AudioListener.pause = false;
            WebAudio.Mute(false);

            IsAudioPause = false;
            OnUnpause?.Invoke();
            AudioManagement.Instance.Log("Unpause Audio");
        }
    }
}