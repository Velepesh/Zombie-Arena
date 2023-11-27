using UnityEngine;
using UnityEngine.UI;
using System;

public class VolumeView : MonoBehaviour
{
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;

    public event Action<float> SfxValueChanged;
    public event Action<float> MusicValueChanged;

    private void OnEnable()
    {
        _sfxSlider.onValueChanged.AddListener(delegate
        {
            UpdateSFXValue(_sfxSlider.value);
        });

        _musicSlider.onValueChanged.AddListener(delegate
        {
            UpdateMusicValue(_musicSlider.value);
        });
    }

    private void OnDisable()
    {
        _sfxSlider.onValueChanged.RemoveListener(delegate
        {
            UpdateSFXValue(_sfxSlider.value);
        });

        _musicSlider.onValueChanged.RemoveListener(delegate
        {
            UpdateMusicValue(_musicSlider.value);
        });
    }

    public void Init(float sfxValue, float musicValue)
    {
        _sfxSlider.value = sfxValue;
        _musicSlider.value = musicValue;

        SetMixersValues(sfxValue, musicValue);
    }

    private void UpdateSFXValue(float value)
    {
        SfxValueChanged?.Invoke(value);
    }

    private void UpdateMusicValue(float value)
    {
        MusicValueChanged?.Invoke(value);
    }

    private void SetMixersValues(float sfxValue, float musicValue)
    {
        UpdateSFXValue(sfxValue);
        UpdateMusicValue(musicValue);
    }
}