using UnityEngine;

public class EnvironmentTime : MonoBehaviour
{
    [SerializeField] private Game _game;

    private void OnEnable()
    {
        _game.GameStarted += OnTimeStarted;
        _game.Continued += OnTimeStarted;
        _game.Reborned += OnTimeStarted;
        _game.GameOver += OnTimeStoped;
        _game.Paused += OnTimeStoped;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnTimeStarted;
        _game.Continued -= OnTimeStarted;
        _game.Reborned -= OnTimeStarted;
        _game.GameOver -= OnTimeStoped;
        _game.Paused -= OnTimeStoped;
    }

    private void OnTimeStarted()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    private void OnTimeStoped()
    {
        AudioListener.pause = true;
        Time.timeScale = 0;
    }
}