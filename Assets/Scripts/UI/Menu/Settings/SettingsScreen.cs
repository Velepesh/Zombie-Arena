using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasFade))]
public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private SensitivitySettings _sensitivitySettings;
    [SerializeField] private VolumeSettings _volumeSettings;

    private CanvasFade _canvasFade;

    public SensitivitySettings SensitivitySettings => _sensitivitySettings;
    public VolumeSettings VolumeSettings => _volumeSettings;

    public event UnityAction<SettingsScreen> Showed;
    public event UnityAction<float> SensitivityUpdated;
    public event UnityAction<float> SFXUpdated;
    public event UnityAction<float> MusicUpdated;

    private void Awake()
    {
        _canvasFade = GetComponent<CanvasFade>();
    }

    private void OnEnable()
    {
        _canvasFade.Showed += OnShowed;
        _sensitivitySettings.SensitivityUpdated += OnSensitivityUpdated;
        _volumeSettings.SfxValueChanged += OnSfxValueChanged;
        _volumeSettings.MusicValueChanged += OnMusicValueChanged;
    }

    private void OnDisable()
    {
        _canvasFade.Showed -= OnShowed;
        _sensitivitySettings.SensitivityUpdated -= OnSensitivityUpdated;
        _volumeSettings.SfxValueChanged -= OnSfxValueChanged;
        _volumeSettings.MusicValueChanged -= OnMusicValueChanged;
    }

    private void OnShowed()
    {
        Showed?.Invoke(this);
    }

    private void OnSensitivityUpdated(float value)
    {
        SensitivityUpdated?.Invoke(value);
    }

    private void OnSfxValueChanged(float value)
    {
        SFXUpdated?.Invoke(value);
    }

    private void OnMusicValueChanged(float value)
    {
        MusicUpdated?.Invoke(value);
    }
}