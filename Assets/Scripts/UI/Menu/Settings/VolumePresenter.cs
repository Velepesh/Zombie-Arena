using System.Collections.Generic;

public class VolumePresenter 
{
    private List<VolumeView> _views = new List<VolumeView>();
    private Volume _model;
    private SettingsSaver _saver;

    public VolumePresenter(List<VolumeView> views, Volume model, SettingsSaver saver)
    {
        _views = views;
        _model = model;
        _saver = saver;
    }

    public void Enable()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            VolumeView view = _views[i];

            view.Init(_model.Sfx, _model.Music);
            view.MusicValueChanged += OnMusicValueChanged;
            view.SfxValueChanged += OnSfxValueChanged;
        }
    }

    public void Disable()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            VolumeView view = _views[i];

            view.MusicValueChanged -= OnMusicValueChanged;
            view.SfxValueChanged -= OnSfxValueChanged;
        }
    }

    private void OnMusicValueChanged(float value)
    {
        _model.SetMusicVolume(value);
        _saver.OnMusicUpdated(value);
    }

    private void OnSfxValueChanged(float value)
    {
        _model.SetSFXVolume(value);
        _saver.OnSFXUpdated(value);
    }
}
