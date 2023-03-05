using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioMixer _mixer;

    private const string VOLUME = "Volume";

    private float _volume => PlayerPrefs.GetFloat(VOLUME, 0);

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(delegate 
        { 
            UpdateVolumeValue(_slider.value); 
        });
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(delegate
        {
            UpdateVolumeValue(_slider.value);
        });
    }

    private void Start()
    {
        Load();
    }

    public void UpdateVolumeValue(float value)
    {
        _mixer.SetFloat(VOLUME, value);

        SaveValue(value);
    }

    private void Load()
    {
        _slider.value = _volume;
        UpdateVolumeValue(_volume);
    }

    private void SaveValue(float value)
    {
        PlayerPrefs.SetFloat(VOLUME, value);
    }
}