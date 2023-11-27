using UnityEngine;

public class LevelCounterSetup : MonoBehaviour
{
    [SerializeField] private LevelCounterView _view;

    private LevelCounterPresenter _presenter;
    private LevelCounter _model;
    private LevelCounterSaver _saver;

    public LevelCounter LevelCounter => _model;

    public void Init()
    {
        _saver = new LevelCounterSaver();
        _model = new LevelCounter(_saver.Level);
        _presenter = new LevelCounterPresenter(_view, _model, _saver);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}
