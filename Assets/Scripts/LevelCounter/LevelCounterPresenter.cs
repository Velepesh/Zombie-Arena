public class LevelCounterPresenter
{
    private LevelCounterView _view;
    private LevelCounter _model;
    private LevelCounterSaver _saver;

    public LevelCounterPresenter(LevelCounterView view, LevelCounter model, LevelCounterSaver saver)
    {
        _view = view;
        _model = model;
        _saver = saver;
    }

    public void Enable()
    {
        _model.LevelIncreased += OnLevelIncreased;

        _view.OnLevelIncreased(_model.Level);
    }

    public void Disable()
    {
        _model.LevelIncreased -= OnLevelIncreased;
    }


    private void OnLevelIncreased(int level)
    {
        _view.OnLevelIncreased(level);
        _saver.OnLevelIncreased(level);
    }
}
