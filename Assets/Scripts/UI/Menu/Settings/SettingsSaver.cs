using System.Collections.Generic;
using UnityEngine;
using YG;

public class SettingsSaver : MonoBehaviour
{
    [SerializeField] private List<SettingsScreen> _settingsScreens;

    private float _sensetivity = 0;
    private float _sfx = 0;
    private float _music = 0;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;

        for (int i = 0; i < _settingsScreens.Count; i++)
        {
            _settingsScreens[i].Showed += OnShowed;
            _settingsScreens[i].SensitivityUpdated += OnSensitivityUpdated;
            _settingsScreens[i].SFXUpdated += OnSFXUpdated;
            _settingsScreens[i].MusicUpdated += OnMusicUpdated;
        }
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;

        for (int i = 0; i < _settingsScreens.Count; i++)
        {
            _settingsScreens[i].Showed -= OnShowed;
            _settingsScreens[i].SensitivityUpdated -= OnSensitivityUpdated;
            _settingsScreens[i].SFXUpdated -= OnSFXUpdated;
            _settingsScreens[i].MusicUpdated -= OnMusicUpdated;
        }
    }


    private void Load()
    {
        _sensetivity = YandexGame.savesData.Sensetivity;
        _sfx = YandexGame.savesData.SFX;
        _music = YandexGame.savesData.Music;

        for (int i = 0; i < _settingsScreens.Count; i++)
            SetSettingsData(_settingsScreens[i]);
    }

    private void Save()
    {
        YandexGame.SaveProgress();
    }

    private void OnShowed(SettingsScreen screen)
    {
        SetSettingsData(screen);
    }

    private void SetSettingsData(SettingsScreen screen)
    {
        screen.SensitivitySettings.Init(_sensetivity);
        screen.VolumeSettings.Init(_sfx, _music);
    }

    private void OnSensitivityUpdated(float value)
    {
        if (_sensetivity == value)
            return;

        _sensetivity = value;
        YandexGame.savesData.Sensetivity = _sensetivity;
        Save();
    }

    private void OnSFXUpdated(float value)
    {
        if (_sfx == value)
            return;

        _sfx = value;
        YandexGame.savesData.SFX = _sfx;
        Save();
    }

    private void OnMusicUpdated(float value)
    {
        if (_music == value)
            return;

        _music = value;
        YandexGame.savesData.Music = _music;
        Save();
    }
}