using UnityEngine;

public class HighscorePresenter : MonoBehaviour
{
    private HighscoreView _view;
    private Highscore _model;
    private HighscoreSaver _saver;

    public HighscorePresenter(HighscoreView view, Highscore model, HighscoreSaver saver)
    {
        _view = view;
        _model = model;
        _saver = saver;
    }

    public void Enable()
    {
        _model.Recorded += OnRecorded;

        _view.ShowRecorded(_model.Value);
    }

    public void Disable()
    {
        _model.Recorded -= OnRecorded;
    }


    private void OnRecorded(int highscore)
    {
        _view.ShowRecorded(highscore);
        _saver.OnRecorded(highscore);
    }
}
