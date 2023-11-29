using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Game : MonoCache
{
    [SerializeField] private GameModeSelector _selector;
    [SerializeField] private CompositionOrder _order;
    [SerializeField] private ZombieTargetsCompositeRoot _targets;
    [SerializeField] private ZombieSpawnerCompositeRoot _zombieSpawnerCompositeRoot;
    [SerializeField] private ScoreSetup _scoreSetup;
    [SerializeField] private Timer _timer;

    private LevelCounter _levelCounter;
    private bool _isPaused;
    private bool _isLose;
    private bool _isWin;

    public int TotalScore => _scoreSetup.Score.TotalScore;
    public int DoubleEarnings => TotalScore * 2;

    public event UnityAction Inited;
    public event UnityAction Won;
    public event UnityAction GameStarted;
    public event UnityAction GameOver;
    public event UnityAction Continued;
    public event UnityAction Paused;
    public event UnityAction Restarted;
    public event UnityAction Reborned;

    private void OnEnable()
    {
        _selector.Selected += OnGameModeSelected;
        _zombieSpawnerCompositeRoot.Ended += OnZombieEnded;
        _targets.TargetDied += OnTargetDied;
    }

    private void OnDisable()
    {
        _selector.Selected -= OnGameModeSelected;
        _zombieSpawnerCompositeRoot.Ended -= OnZombieEnded;
        _targets.TargetDied -= OnTargetDied;
    }

    public void Init(LevelCounter levelCounter)
    {
        if (levelCounter == null)
            throw new ArgumentNullException(nameof(levelCounter));

        _levelCounter = levelCounter;
    }

    public void Continue()
    {
        _isPaused = false;
        Continued?.Invoke();
    }

    public void NextLevel()
    {
        EndLevel();
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

    private void OnGameModeSelected(GameMode gameMode)
    {
        StartLevel(gameMode);
    }

    private void StartLevel(GameMode gameMode)
    {
        ZombiesSpawner zombiesSpawner = _zombieSpawnerCompositeRoot.InitSpawner(gameMode);
        _scoreSetup.Init(zombiesSpawner);
        _order.Compose();
        _timer.StartTimer();
        GameStarted?.Invoke();
        MetricaSender.LevelStart(_levelCounter.Level);
    }

    private void OnZombieEnded()
    {
        Win();
    }

    private void EndLevel()
    {
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