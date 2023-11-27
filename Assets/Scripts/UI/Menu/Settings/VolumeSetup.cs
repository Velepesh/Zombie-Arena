using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSetup : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private List<VolumeView> _volumeViews;

    private Volume _model;
    private VolumePresenter _presenter;

    public void Init(SettingsSaver saver)
    {
        _model = new Volume(_mixer, saver.Sfx, saver.Music);

        _presenter = new VolumePresenter(_volumeViews, _model, saver);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}