using UnityEngine;
using System.Collections;
using Plugins.Audio.Core;

public class EnvironmentTime : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private float _delayBeforeGameOver = .8f;

    private bool _isFocused = true;

    private void Start()
    {
        OnTimeStarted();
    }

    private void OnEnable()
    {
        AppFocusHandle.OnFocus += Focus;
        AppFocusHandle.OnUnfocus += UnFocus;
        _game.GameStarted += OnTimeStarted;
        _game.Continued += OnTimeStarted;
        _game.Reborned += OnTimeStarted;
        _game.GameOver += OnGameOver;
        _game.Paused += OnTimeStoped;
    }

    private void OnDisable()
    {
        AppFocusHandle.OnFocus -= Focus;
        AppFocusHandle.OnUnfocus -= UnFocus;
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

    private void Focus()
    {
        if (_isFocused == true)
            return;

        _isFocused = true;
        OnTimeStarted();
    }

    private void UnFocus()
    {
        if (_isFocused == false)
            return;

        _isFocused = false;
        OnTimeStoped();
    }
}