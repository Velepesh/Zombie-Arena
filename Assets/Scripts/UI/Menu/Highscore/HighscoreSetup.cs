using UnityEngine;

public class HighscoreSetup : MonoBehaviour
{
    [SerializeField] private HighscoreView _view;

    private HighscorePresenter _presenter;
    private Highscore _model;
    private HighscoreSaver _saver;

    public Highscore Model => _model;

    public void Init()
    {
        _saver = new HighscoreSaver();
        _model = new Highscore(_saver.Highscore);
        _presenter = new HighscorePresenter(_view, _model, _saver);
        _presenter.Enable();
    }

    private void OnDestroy()
    {
        _presenter.Disable();
    }
}