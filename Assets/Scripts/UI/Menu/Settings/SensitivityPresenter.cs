using System.Collections.Generic;

public class SensitivityPresenter
{
    private List<SensitivitySettings> _settings = new List<SensitivitySettings>();
    private Sensitivity _model;

    public SensitivityPresenter(List<SensitivitySettings> settings, Sensitivity model)
    {
        _settings = settings;
        _model = model;
    }

    public void Enable()
    {
        for (int i = 0; i < _settings.Count; i++)
            _settings[i].SensitivityUpdated += OnSensitivityUpdated;
    }

    public void Disable()
    {
        for (int i = 0; i < _settings.Count; i++)
            _settings[i].SensitivityUpdated -= OnSensitivityUpdated;
    }

    private void OnSensitivityUpdated(float value)
    {
        _model.UpdateSensitivityVector(value);
    }
}