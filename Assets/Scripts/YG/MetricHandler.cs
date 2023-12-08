using UnityEngine;

public class MetricHandler : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Timer _timer;

    private void OnEnable()
    {
        _game.Started += OnStarted;
        _game.Won += OnWon;
        _game.GameOver += OnGameOver;
        _game.Restarted += OnRestarted;
    }

    private void OnDisable()
    {
        _game.Started -= OnStarted;
        _game.Won -= OnWon;
        _game.GameOver -= OnGameOver;
        _game.Restarted -= OnRestarted;
    }

    private void OnStarted()
    {
        MetricaSender.LevelStart(_game.CurrentLevel);
    }

    private void OnWon()
    {
        MetricaSender.LevelComplete(_game.CurrentLevel, _timer.SpentTime);
    }

    private void OnGameOver()
    {
        MetricaSender.Fail(_game.CurrentLevel, _timer.SpentTime);
    }

    private void OnRestarted()
    {
        if(_game.IsLose == false && _game.IsWon == false)
            MetricaSender.Restart(_game.CurrentLevel);
    }
}
