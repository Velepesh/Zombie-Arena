using UnityEngine;
using UnityEngine.Events;

public class UIEvents : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private UnityEvent _onGameStarted;
    [SerializeField] private UnityEvent _onGameOver;
    [SerializeField] private UnityEvent _onSettings;
    [SerializeField] private UnityEvent _onControls;
    [SerializeField] private UnityEvent _onRestarted;
    [SerializeField] private UnityEvent _onContinued;
    [SerializeField] private UnityEvent _onPaused;

    private void OnEnable()
    {
        _game.GameStarted += () => _onGameStarted?.Invoke();
        _game.GameOver += () => _onGameOver?.Invoke();
        _game.Settings += () => _onSettings?.Invoke();
        _game.Controls += () => _onControls?.Invoke();
        _game.Restarted += () => _onRestarted?.Invoke();
        _game.Continued += () => _onContinued?.Invoke();
        _game.Paused += () => _onPaused?.Invoke();
    }

    private void OnDisable()
    {
        _game.GameStarted -= () => _onGameStarted?.Invoke();
        _game.GameOver -= () => _onGameOver?.Invoke();
        _game.Settings -= () => _onSettings?.Invoke();
        _game.Controls -= () => _onControls?.Invoke();
        _game.Restarted -= () => _onRestarted?.Invoke();
        _game.Continued -= () => _onContinued?.Invoke();
        _game.Paused -= () => _onPaused?.Invoke();
    }
}