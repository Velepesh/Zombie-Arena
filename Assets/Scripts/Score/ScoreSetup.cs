using UnityEngine;
using UnityEngine.Events;

public class ScoreSetup : MonoBehaviour
{
    [SerializeField] private ZombieSpawner _zombieSpawner;
    [SerializeField] private AnimatedScoreView _view;

    private ScorePresenter _presenter;
    private Score _model;

    public Score Score => _model;

    private void Awake()
    {
        _model = new Score();
        _presenter = new ScorePresenter(_view, _model, _zombieSpawner);
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