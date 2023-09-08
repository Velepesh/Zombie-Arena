using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class SettingsSaver : MonoBehaviour, ISaver
{
    [SerializeField] private List<SettingsScreen> _settingsScreens;

    readonly string _sensitivityPath = "/sensitivity.json";
    readonly string _sfxPath = "/sfx.json";
    readonly string _musicPath = "/music.json";

    private IDataService _dataService;
    private SettingsData _sensitivityData;
    private SettingsData _sfxData;
    private SettingsData _musicData;

    private void Awake()
    {
        _dataService = new JsonDataService();
        LoadData();
    }

    private void OnEnable()
    {
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
        for (int i = 0; i < _settingsScreens.Count; i++)
        {
            _settingsScreens[i].Showed -= OnShowed;
            _settingsScreens[i].SensitivityUpdated -= OnSensitivityUpdated;
            _settingsScreens[i].SFXUpdated -= OnSFXUpdated;
            _settingsScreens[i].MusicUpdated -= OnMusicUpdated;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _settingsScreens.Count; i++)
            SetSettingsData(_settingsScreens[i]);
    }

    public void LoadData()
    {
        _sensitivityData = LoadSettings(_sensitivityPath);
        _sfxData = LoadSettings(_sfxPath);
        _musicData = LoadSettings(_musicPath);
    }

    public void SaveData<SettingsData>(string path, SettingsData data)
    {
        _dataService.SaveData(path, data);
    }

    private void OnShowed(SettingsScreen screen)
    {
        SetSettingsData(screen);
    }

    private void SetSettingsData(SettingsScreen screen)
    {
        screen.SensitivitySettings.Init(_sensitivityData.Value);
        screen.VolumeSettings.Init(_sfxData.Value, _musicData.Value);
    }

    private void OnSensitivityUpdated(float value)
    {
        _sensitivityData.Value = value;
        SaveData(_sensitivityPath, _sensitivityData);
    }

    private void OnSFXUpdated(float value)
    {
        _sfxData.Value = value;
        SaveData(_sfxPath, _sfxData);
    }

    private void OnMusicUpdated(float value)
    {
        _musicData.Value = value;
        SaveData(_musicPath, _musicData);
    }

    private SettingsData LoadSettings(string path)
    {
        SettingsData data = _dataService.LoadData<SettingsData>(path);

        if (data == null)
            data = new SettingsData();

        return data;
    }
}