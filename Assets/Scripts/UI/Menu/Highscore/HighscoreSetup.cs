using UnityEngine;

public class HighscoreSetup : MonoBehaviour
{
    [SerializeField] private Highscore _model;
    [SerializeField] private HighscoreView _view;

    private HighscorePresenter _presenter;
    private HighscoreSaver _saver;

    public Highscore Model => _model;

    public void Init()
    {
        _saver = new HighscoreSaver();
        _model.Init(_saver.Highscore);
        _presenter = new HighscorePresenter(_view, _model, _saver);
        _presenter.Enable();
    }

    private void OnDisable()
    {
        _presenter.Disable();
    }
}