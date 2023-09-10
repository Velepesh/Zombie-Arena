using UnityEngine;
using UnityEngine.Events;

public class UIEvents : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private UnityEvent _onGameStarted;
    [SerializeField] private UnityEvent _onWon;
    [SerializeField] private UnityEvent _onGameOver;
    [SerializeField] private UnityEvent _onContinued;
    [SerializeField] private UnityEvent _onPaused;

    private void OnEnable()
    {
        _game.GameStarted += () => _onGameStarted?.Invoke();
        _game.Won += () => _onWon?.Invoke();
        _game.GameOver += () => _onGameOver?.Invoke();
        _game.Continued += () => _onContinued?.Invoke();
        _game.Paused += () => _onPaused?.Invoke();
    }

    private void OnDisable()
    {
        _game.GameStarted -= () => _onGameStarted?.Invoke();
        _game.Won -= () => _onWon?.Invoke();
        _game.GameOver -= () => _onGameOver?.Invoke();
        _game.Continued -= () => _onContinued?.Invoke();
        _game.Paused -= () => _onPaused?.Invoke();
    }
}