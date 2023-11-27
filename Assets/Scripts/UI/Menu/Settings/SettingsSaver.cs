using YG;

public class SettingsSaver
{
    public float Sensetivity => YandexGame.savesData.Sensetivity;
    public float Sfx => YandexGame.savesData.SFX;
    public float Music => YandexGame.savesData.Music;

    private float _currentSensetivity;
    private float _currentSFX;
    private float _currentMusic;

    public SettingsSaver()
    {
        Load();
    }

    private void Load()
    {
        _currentSensetivity = YandexGame.savesData.Sensetivity;
        _currentSFX = YandexGame.savesData.SFX;
        _currentMusic = YandexGame.savesData.Music;
    }

    public void Save()
    {
        YandexGame.savesData.Sensetivity = _currentSensetivity;
        YandexGame.savesData.Music = _currentMusic;
        YandexGame.savesData.SFX = _currentSFX;
        
        YandexGame.SaveProgress();
    }

    public void OnSensitivityUpdated(float value)
    {
        SetCurrentValue(ref _currentSensetivity, value);
    }

    public void OnSFXUpdated(float value)
    {
        SetCurrentValue(ref _currentSFX, value);
    }

    public void OnMusicUpdated(float value)
    {
        SetCurrentValue(ref _currentMusic, value);
    }

    private void SetCurrentValue(ref float currentValue, float newValue)
    {
        if (newValue == currentValue)
            return;

        currentValue = newValue;
    }
}