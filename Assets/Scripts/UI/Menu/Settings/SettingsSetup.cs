using UnityEngine;
using System.Collections.Generic;

public class SettingsSetup : MonoBehaviour
{
    [SerializeField] private List<SettingsScreen> _settingsScreens;
    [SerializeField] private SensitivitySetup _sensitivitySetup;
    [SerializeField] private VolumeSetup _volumeSetup;

    private SettingPresenter _presenter;
    private SettingsSaver _saver;

    public void Init()
    {
        _saver = new SettingsSaver();
        _sensitivitySetup.Init(_saver);
        _volumeSetup.Init(_saver);
        _presenter = new SettingPresenter(_settingsScreens, _saver);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}