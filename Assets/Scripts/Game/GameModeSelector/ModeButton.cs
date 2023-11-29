using System;
using UnityEngine;
using UnityEngine.UI;

public class ModeButton : MonoBehaviour
{
    [SerializeField] private GameMode _gameMode;
    [SerializeField] private Button _modeButton;

    public event Action<GameMode> ModeButtonClicked;

    private void OnEnable()
    {
        _modeButton.onClick.AddListener(OnModeButtonClicked);
    }

    private void OnDisable()
    {
        _modeButton.onClick.RemoveListener(OnModeButtonClicked);
    }

    private void OnModeButtonClicked()
    {
        ModeButtonClicked?.Invoke(_gameMode);
    }
}