using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private AudioMixer _mixer;

    readonly string SFX = "SFX";
    readonly string MUSIC = "Music";

    public event UnityAction<float> SfxValueChanged;
    public event UnityAction<float> MusicValueChanged;

    private void OnEnable()
    {
        _sfxSlider.onValueChanged.AddListener(delegate
        {
            UpdateMixerValue(SFX, _sfxSlider.value);
        });

        _musicSlider.onValueChanged.AddListener(delegate
        {
            UpdateMixerValue(MUSIC, _musicSlider.value);
        });
    }

    private void OnDisable()
    {
        _sfxSlider.onValueChanged.RemoveListener(delegate
        {
            UpdateMixerValue(SFX, _sfxSlider.value);
        });

        _musicSlider.onValueChanged.RemoveListener(delegate
        {
            UpdateMixerValue(MUSIC, _musicSlider.value);
        });
    }

    public void Init(float sfxValue, float musicValue)
    {
        _sfxSlider.value = sfxValue;
        _musicSlider.value = musicValue;

        SetMixersValues(sfxValue, musicValue);
    }

    private void UpdateMixerValue(string key, float value)
    {
        _mixer.SetFloat(key, value);

        if (key == SFX)
            SfxValueChanged?.Invoke(value);
        else
            MusicValueChanged?.Invoke(value);
    }

    private void SetMixersValues(float sfxValue, float musicValue)
    {
        UpdateMixerValue(SFX, sfxValue);
        UpdateMixerValue(MUSIC, musicValue);
    }
}