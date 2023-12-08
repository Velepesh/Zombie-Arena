using UnityEngine;

public class ScoreSetup : MonoBehaviour
{
    [SerializeField] private ScoreView _view;

    private ScorePresenter _presenter;
    private Score _model;

    public int TotalScore => _model.TotalScore;

    public void Init(ZombiesSpawner spawner)
    {
        _model = new Score();
        _presenter = new ScorePresenter(_view, _model, spawner);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        if(_presenter != null)
            _presenter.Disable();
    }
}