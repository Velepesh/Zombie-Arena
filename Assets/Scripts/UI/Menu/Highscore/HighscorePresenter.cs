public class HighscorePresenter
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

        UpdateView(_model.Value);
    }

    public void Disable()
    {
        _model.Recorded -= OnRecorded;
    }

    private void OnRecorded(int highscore)
    {
        UpdateView(highscore);
        _saver.OnRecorded(highscore);
    }

    private void UpdateView(int highscore)
    {
        _view.ShowRecorded(highscore);
        _view.SetMedalIcon(_model);
    }
}