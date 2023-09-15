using System;
using System.Collections.Generic;
using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class SensitivitySetup : MonoBehaviour
{
    [SerializeField] private CameraLook _cameraLook;
    [SerializeField] private List<SensitivitySettings> _settings;

    private Sensitivity _model;
    private SensitivityPresenter _presenter;

    private void Awake()
    {
        if (_cameraLook.Sensitivity == null)
            throw new ArgumentNullException(nameof(_cameraLook.Sensitivity));

        _model = _cameraLook.Sensitivity;

        _presenter = new SensitivityPresenter(_settings, _model);
    }

    private void OnEnable()
    {
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}