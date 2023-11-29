using System;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelector : MonoBehaviour
{
    [SerializeField, NotNull] private List<ModeButton> _modeButtons;

    public event Action<GameMode> Selected;

    private void OnEnable()
    {
        for (int i = 0; i < _modeButtons.Count; i++)
            _modeButtons[i].ModeButtonClicked += OnModeButtonClicked;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _modeButtons.Count; i++)
            _modeButtons[i].ModeButtonClicked -= OnModeButtonClicked;
    }

    private void OnModeButtonClicked(GameMode gameMode)
    {
        Selected?.Invoke(gameMode);
    }
}