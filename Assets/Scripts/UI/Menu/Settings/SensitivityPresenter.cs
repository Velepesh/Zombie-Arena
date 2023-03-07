public class SensitivityPresenter
{
    private SensitivitySettings _settings;
    private Sensitivity _model;

    public SensitivityPresenter(SensitivitySettings settings, Sensitivity model)
    {
        _settings = settings;
        _model = model;
    }

    public void Enable()
    {
        _settings.SensitivityUpdated += OnSensitivityUpdated;
    }

    public void Disable()
    {
        _settings.SensitivityUpdated -= OnSensitivityUpdated;
    }

    private void OnSensitivityUpdated(float value)
    {
        _model.UpdateSensitivityVector(value);
    }
}