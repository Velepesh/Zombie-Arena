using System.Collections.Generic;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class SensitivitySetup : MonoBehaviour
{
    [SerializeField] private CameraLook _cameraLook;
    [SerializeField] private List<SensitivityView> _sensitivityViews;

    private Sensitivity _model;
    private SensitivityPresenter _presenter;

    public void Init(SettingsSaver saver)
    {
        _model = new Sensitivity(saver.Sensetivity);
        _cameraLook.Init(_model);

        _presenter = new SensitivityPresenter(_sensitivityViews, _model, saver);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}