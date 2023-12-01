using UnityEngine;
using UnityEngine.Events;

public class UIEvents : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private UnityEvent _onGameStarted;
    [SerializeField] private UnityEvent _onPaused;
    [SerializeField] private UnityEvent _onUnpaused;
    [SerializeField] private UnityEvent _onWon;
    [SerializeField] private UnityEvent _onFirstLoss;
    [SerializeField] private UnityEvent _onReborned;
    [SerializeField] private UnityEvent _onContinued;
    [SerializeField] private UnityEvent _onGameOver;

    private void OnEnable()
    {
        _game.Started += () => _onGameStarted?.Invoke();
        _game.Paused += () => _onPaused?.Invoke();
        _game.Unpaused += () => _onUnpaused?.Invoke();
        _game.Won += () => _onWon?.Invoke();
        _game.FirstLoss += () => _onFirstLoss?.Invoke();
        _game.Reborned += () => _onReborned?.Invoke();
        _game.Continued += () => _onContinued?.Invoke();
        _game.GameOver += () => _onGameOver?.Invoke();
    }

    private void OnDisable()
    {
        _game.Started -= () => _onGameStarted?.Invoke();
        _game.Paused -= () => _onPaused?.Invoke();
        _game.Unpaused -= () => _onUnpaused?.Invoke();
        _game.Won -= () => _onWon?.Invoke();
        _game.FirstLoss -= () => _onFirstLoss?.Invoke();
        _game.Reborned -= () => _onReborned?.Invoke();
        _game.Continued -= () => _onContinued?.Invoke();
        _game.GameOver -= () => _onGameOver?.Invoke();
    }
}