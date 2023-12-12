using System;
using Plugins.Audio.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Audio.Examples
{
    public class AudioTestUI : MonoBehaviour
    {
        [SerializeField] private AudioTest _audioTest;

        [SerializeField] private TMP_Dropdown _providerDropdown;

        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _musicPitchSlider;
        [SerializeField] private Slider _soundsVolumeSlider;
        [SerializeField] private Slider _soundsPitchSlider;

        [SerializeField] private Button _playMusicButton;
        [SerializeField] private Button _stopMusicButton;
        [SerializeField] private Button _pauseMusicButton;
        [SerializeField] private Button _unPauseMusicButton;
        [SerializeField] private Button _playSoundsButtton;

        private void OnEnable()
        {
            _musicVolumeSlider.onValueChanged.AddListener(_audioTest.SetMusicVolume);
            _musicPitchSlider.onValueChanged.AddListener(_audioTest.SetMusicPitch);
            _soundsVolumeSlider.onValueChanged.AddListener(_audioTest.SetSoundsVolume);
            _soundsPitchSlider.onValueChanged.AddListener(_audioTest.SetSoundPitch);
            
            _playMusicButton.onClick.AddListener(_audioTest.PlayMusic);
            _stopMusicButton.onClick.AddListener(_audioTest.StopMusic);
            _pauseMusicButton.onClick.AddListener(_audioTest.PauseMusic);
            _unPauseMusicButton.onClick.AddListener(_audioTest.UnPauseMusic);
            _playSoundsButtton.onClick.AddListener(_audioTest.PlaySound);
            
            _providerDropdown.onValueChanged.AddListener(AudioProviderChanged);
        }

        private void OnDisable()
        {
            _musicVolumeSlider.onValueChanged.RemoveListener(_audioTest.SetMusicVolume);
            _musicPitchSlider.onValueChanged.RemoveListener(_audioTest.SetMusicPitch);
            _soundsVolumeSlider.onValueChanged.RemoveListener(_audioTest.SetSoundsVolume);
            _soundsPitchSlider.onValueChanged.RemoveListener(_audioTest.SetSoundPitch);
            
            _playMusicButton.onClick.RemoveListener(_audioTest.PlayMusic);
            _stopMusicButton.onClick.RemoveListener(_audioTest.StopMusic);
            _pauseMusicButton.onClick.RemoveListener(_audioTest.PauseMusic);
            _unPauseMusicButton.onClick.RemoveListener(_audioTest.UnPauseMusic);
            _playSoundsButtton.onClick.RemoveListener(_audioTest.PlaySound);
            
            _providerDropdown.onValueChanged.RemoveListener(AudioProviderChanged);
        }

        private void AudioProviderChanged(int index)
        {
            AudioProviderType providerType = index == 0 ? AudioProviderType.Unity : AudioProviderType.JS;

            _audioTest.SetAudioProvider(providerType);
        }
    }
}