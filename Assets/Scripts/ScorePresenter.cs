public class ScorePresenter
{
    private AnimatedScoreView _view;
    private Score _model;
    private ZombieSpawner _zombieSpawner;

    public ScorePresenter(AnimatedScoreView view, Score model, ZombieSpawner zombieSpawner)
    {
        _view = view;
        _model = model;
        _zombieSpawner = zombieSpawner;
    }

    public void Enable()
    {
        _model.Added += OnAdded;
        _zombieSpawner.ZombieDied += OnZombieDied;

        _view.SetScoreValue(_model.TotalScore);
    }

    public void Disable()
    {
        _model.Added -= OnAdded;
        _zombieSpawner.ZombieDied -= OnZombieDied;
    }

    private void OnZombieDied(Zombie zombie)
    {
        _model.AddScore(zombie);
    }

    private void OnAdded(int addedScore)
    {
        _view.SetAmount(addedScore, _model.TotalScore);
    }
}