using UnityEngine;
using System.Collections;
using Plugins.Audio.Core;

public class EnvironmentTime : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private AudioPauseHandler _audioPauseHandler;
    [SerializeField] private float _delayBeforeGameOver = .1f;
    [SerializeField] private RebornButton _rebornButton;

    private bool _isAds = false;
    private bool _isReborningAds = false;
    private bool _isFocused = true;

    private void Start()
    {
        Unpause();
    }

    private void OnEnable()
    {
        AppFocusHandle.OnFocus += Focus;
        AppFocusHandle.OnUnfocus += UnFocus;
        _rebornButton.RebornButtonClicked += OnRebornButtonClicked;
        _game.GameStarted += OnGameStarted;
        _game.Continued += OnContinued;
        _game.Reborned += OnReborned;
        _game.GameOver += OnGameOver;
        _game.Paused += OnPaused;
    }

    private void OnDisable()
    {
        AppFocusHandle.OnFocus -= Focus;
        AppFocusHandle.OnUnfocus -= UnFocus;
        _rebornButton.RebornButtonClicked -= OnRebornButtonClicked;
        _game.GameStarted -= OnGameStarted;
        _game.Continued -= OnContinued;
        _game.Reborned -= OnReborned;
        _game.GameOver -= OnGameOver;
        _game.Paused -= OnPaused;
    }

    public void OpenAdsPause()
    {
        if (_isAds == true)
            return;

        _isAds = true;

        _audioPauseHandler.Pause();
        StopTime();
    }

    public void CloseAdsUnpause()
    {
        if (_isAds == false)
            return;

        if (_isReborningAds)
            return;

        _isAds = false;

        _audioPauseHandler.Unpause();
        StartTime();
    }

    private void Pause()
    {
        _audioPauseHandler.Pause();
        StopTime();
    }

    private void Unpause()
    {
        _audioPauseHandler.Unpause();
        StartTime();
    }

    private void Focus()
    {
        if (_isFocused == true)
            return;

        _isFocused = true;

        if (_isAds == false)
            Unpause();
    }

    private void UnFocus()
    {
        if (_isFocused == false)
            return;

        _isFocused = false;
        Pause();
    }

    private void OnRebornButtonClicked()
    {
        _isReborningAds = true;
    }

    private void OnGameOver()
    {
        StartCoroutine(DelayBeforePause());
    }

    private IEnumerator DelayBeforePause()
    {
        yield return new WaitForSeconds(_delayBeforeGameOver);
        Pause();
    }

    private void OnContinued()
    {
        Unpause();
    }

    private void OnGameStarted()
    {
        Unpause();
    }

    private void OnPaused()
    {
        Pause();
    }

    private void OnReborned()
    {
        _isReborningAds = false;
        CloseAdsUnpause();
    }

    private void StartTime()
    {
        Time.timeScale = 1;
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }
}