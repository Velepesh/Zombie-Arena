﻿using UnityEngine;
using System.Collections;
using Plugins.Audio.Core;

public class EnvironmentTime : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private float _delayBeforeGameOver = .1f;
    [SerializeField] private RebornButton _rebornButton;

    private bool _isAds = false;
    private bool _isPaused = false;
    private bool _isReborningAds = false;
    private bool _isFocused = true;
    private Coroutine _delayBeforePauseCoroutine;

    private void Start()
    {
        Pause();
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

        StopTime();
    }

    public void CloseAdsUnpause()
    {
        if (_isAds == false)
            return;

        if (_isReborningAds)
            return;

        _isAds = false;

        StartTime();
    }

    private void Pause()
    {
        if (_delayBeforePauseCoroutine != null)
            StopCoroutine(_delayBeforePauseCoroutine);

        StopTime();
    }

    private void Unpause()
    {
        if (_isPaused)
            return;

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

    private IEnumerator DelayBeforePause()
    {
        yield return new WaitForSeconds(_delayBeforeGameOver);
        Pause();
    }

    private void OnContinued()
    {
        _isPaused = false;
        Unpause();
    }

    private void OnGameStarted()
    {
        Unpause();
    }

    private void OnPaused()
    {
        _isPaused = true;
        Pause();
    }

    private void OnRebornButtonClicked()
    {
        _isReborningAds = true;
    }

    private void OnGameOver()
    {
        _delayBeforePauseCoroutine = StartCoroutine(DelayBeforePause());
    }

    private void OnReborned()
    {
        _isReborningAds = false;
        CloseAdsUnpause();
    }

    private void StartTime()
    {
        Time.timeScale = 1;
        AudioPauseHandler.Instance.Unpause();
    }

    private void StopTime()
    {
        AudioPauseHandler.Instance.Pause();
        Time.timeScale = 0;
    }
}