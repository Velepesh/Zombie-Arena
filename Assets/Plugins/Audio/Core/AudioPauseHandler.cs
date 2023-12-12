using System;
using UnityEngine;

namespace Plugins.Audio.Core
{
    public class AudioPauseHandler : MonoBehaviour
    {
        public static AudioPauseHandler Instance => _instance;
        
        private static AudioPauseHandler _instance;
        
        private bool _isFocused = true;
        private bool _isAds = false;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Focus();
            }
            else
            {
                UnFocus();
            }
        }
#endif

        private void OnEnable()
        {
            AppFocusHandle.OnFocus += Focus;
            AppFocusHandle.OnUnfocus += UnFocus;
        }

        private void OnDisable()
        {
            AppFocusHandle.OnFocus -= Focus;
            AppFocusHandle.OnUnfocus -= UnFocus;
        }

        private void Focus()
        {
            if (_isFocused == true)
            {
                return;
            }
            
            _isFocused = true;

            if (_isAds == false)
            {
                AudioManagement.Instance.Unpause();
            }
        }

        private void UnFocus()
        {
            if (_isFocused == false)
            {
                return;
            }
            
            _isFocused = false;
            AudioManagement.Instance.Pause();
        }

        public void PauseAudio()
        {
            if (_isAds == true)
            {
                return;
            }

            _isAds = true;
            AudioManagement.Instance.Pause();
        }

        public void UnpauseAudio()
        {
            if (_isAds == false)
            {
                return;
            }

            _isAds = false;

            if (_isFocused == true)
            {
                AudioManagement.Instance.Unpause();
            }
        }
    }
}
