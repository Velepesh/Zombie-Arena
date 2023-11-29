using UnityEngine;
using System;

public class ScoreSetup : MonoBehaviour
{
    [SerializeField] private AnimatedScoreView _view;

    private ScorePresenter _presenter;
    private Score _model;

    public Score Score => _model;

    public void Init(ZombiesSpawner zombiesSpawner)
    {
        if(zombiesSpawner == null)
            throw new ArgumentNullException(nameof(zombiesSpawner));

        _model = new Score();
        _presenter = new ScorePresenter(_view, _model, zombiesSpawner);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}