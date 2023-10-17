using UnityEngine;
using System.Collections;

public class EnvironmentTime : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private float _delayBeforeGameOver = .8f;

    private void Start()
    {
        OnTimeStarted();
    }

    private void OnEnable()
    {
        _game.GameStarted += OnTimeStarted;
        _game.Continued += OnTimeStarted;
        _game.Reborned += OnTimeStarted;
        _game.GameOver += OnGameOver;
        _game.Paused += OnTimeStoped;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnTimeStarted;
        _game.Continued -= OnTimeStarted;
        _game.Reborned -= OnTimeStarted;
        _game.GameOver -= OnGameOver;
        _game.Paused -= OnTimeStoped;
    }

    private void OnGameOver()
    {
        StartCoroutine(DelayBeforeStopTime());
    }

    private IEnumerator DelayBeforeStopTime()
    {
        yield return new WaitForSeconds(_delayBeforeGameOver);
        OnTimeStoped();
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