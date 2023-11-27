using System.Collections.Generic;

public class SettingPresenter
{
    private List<SettingsScreen> _screens = new List<SettingsScreen>();
    private SettingsSaver _saver;

    public SettingPresenter(List<SettingsScreen> screens, SettingsSaver saver)
    {
        _screens = screens;
        _saver = saver;
    }

    public void Enable()
    {
        for (int i = 0; i < _screens.Count; i++)
            _screens[i].BackButtonClicked += OnBackButtonClicked;
    }

    public void Disable()
    {
        for (int i = 0; i < _screens.Count; i++)
            _screens[i].BackButtonClicked -= OnBackButtonClicked;
    }

    private void OnBackButtonClicked()
    {
        _saver.Save();

        for (int i = 0; i < _screens.Count; i++)
            _screens[i].UpdateSettingSlidersValue(_saver.Sensetivity, _saver.Music, _saver.Sfx);
    }
}