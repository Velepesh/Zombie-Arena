using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using YG;

public class Game : MonoCache
{
    [SerializeField] private CompositionOrder _order;
    [SerializeField] private ZombieTargetsCompositeRoot _targets;
    [SerializeField] private ZombieSpawnerCompositeRoot _zombieSpawnerCompositeRoot;
    [SerializeField] private ScoreSetup _scoreSetup;
    [SerializeField] private LevelCounter _levelCounter;
    [SerializeField] private Timer _timer;

    private int _defaultEarnings => TotalScore;
    private bool _isPaused;
    private bool _isLose;
    private bool _isWin;

    public int TotalScore => _scoreSetup.Score.TotalScore;
    public int DoubleEarnings => TotalScore * 2;

    public event UnityAction Won;
    public event UnityAction GameStarted;
    public event UnityAction GameOver;
    public event UnityAction Continued;
    public event UnityAction Paused;
    public event UnityAction Restarted;
    public event UnityAction Reborned;
    public event UnityAction<int> Earned;


    private void OnEnable()
    {
        _zombieSpawnerCompositeRoot.Ended += OnZombieEnded;
        _targets.TargetDied += OnTargetDied;
    }

    private void OnDisable()
    {
        _zombieSpawnerCompositeRoot.Ended -= OnZombieEnded;
        _targets.TargetDied -= OnTargetDied;
    }

    public void StartLevel()
    {
        _timer.StartTimer();
        _order.Compose();
        GameStarted?.Invoke();
        MetricaSender.LevelStart(_levelCounter.Level);
    }

    public void Continue()
    {
        _isPaused = false;
        Continued?.Invoke();
    }

    public void NextLevel()
    {
        EndLevel(_defaultEarnings);
    }

    public void NextLevelDoubleEarnings()
    {
        EndLevel(DoubleEarnings);
    }

    public void OnTryPause(InputAction.CallbackContext context)
    {
        if (_isPaused == false)
        {
            Paused?.Invoke();
            _isPaused = true;
        }
    }

    public void Restart()
    {
        if(_isLose)
            MetricaSender.Fail(_levelCounter.Level, _timer.SpentTime);
        else if(_isWin)
            MetricaSender.LevelComplete(_levelCounter.Level, _timer.SpentTime);
        else
            MetricaSender.Restart(_levelCounter.Level);

        DOTween.Clear(true);       
        Restarted?.Invoke();
    }

    public void Reborn()
    {
        _isLose = false;
        _timer.StopTimer();
        Reborned?.Invoke();
    }

    private void OnZombieEnded()
    {
        Win();
    }

    private void EndLevel(int money)
    {
        Earned?.Invoke(money);
        Restart();
    }

    private void Win()
    {
        _isWin = true;
        _timer.StopTimer();
        _levelCounter.IncreaseLevel();
        Won?.Invoke();
    }

    private void OnTargetDied()
    {
        Lose();
    }

    private void Lose()
    {
        _timer.StopTimer();
        _isLose = true;
        GameOver?.Invoke();
    }
}