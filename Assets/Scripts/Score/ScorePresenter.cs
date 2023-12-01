public class ScorePresenter
{
    private ScoreView _view;
    private Score _model;
    private ZombiesSpawner _spawner;

    public ScorePresenter(ScoreView view, Score model, ZombiesSpawner spawner)
    {
        _view = view;
        _model = model;
        _spawner = spawner;
    }

    public void Enable()
    {
        _model.Added += OnAdded;
        _spawner.ZombieDied += OnZombieDied;
        _view.SetScoreValue(_model.TotalScore);
    }

    public void Disable()
    {
        _spawner.ZombieDied -= OnZombieDied;
        _model.Added -= OnAdded;
    }

    private void OnAdded(int addedScore)
    {
        _view.SetAmount(addedScore, _model.TotalScore);
    }

    private void OnZombieDied(Zombie zombie)
    {
       _model.AddScore(zombie);
    }
}