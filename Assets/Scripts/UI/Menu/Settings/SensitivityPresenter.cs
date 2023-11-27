using System.Collections.Generic;

public class SensitivityPresenter
{
    private List<SensitivityView> _views = new List<SensitivityView>();
    private Sensitivity _model;
    private SettingsSaver _saver;

    public SensitivityPresenter(List<SensitivityView> views, Sensitivity model, SettingsSaver saver)
    {
        _views = views;
        _model = model;
        _saver = saver;
    }

    public void Enable()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            _views[i].Init(_model.Value);
            _views[i].SensitivityUpdated += OnSensitivityUpdated;
        }
    }

    public void Disable()
    {
        for (int i = 0; i < _views.Count; i++)
            _views[i].SensitivityUpdated -= OnSensitivityUpdated;
    }

    private void OnSensitivityUpdated(float value)
    {
        _model.UpdateSensitivityVector(value);
        _saver.OnSensitivityUpdated(value);
    }
}