using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    [SerializeField] private ZombieTargetsCompositeRoot _targets;
    [SerializeField] private ZombieSpawner _zombieSpawner;
    [SerializeField] private ScoreSetup _scoreSetup;

    private bool _isGameOver;
    private bool _isWin;

    public Score Score => _scoreSetup.Score;

    public event UnityAction Won;
    public event UnityAction GameStarted;
    public event UnityAction GameOver;
    public event UnityAction Continued;
    public event UnityAction Paused;
    public event UnityAction Restarted;

    private void OnEnable()
    {
        _zombieSpawner.Ended += OnZombieEnded;
        _targets.TargetDied += OnDied;
    }

    private void OnDisable()
    {
        _zombieSpawner.Ended -= OnZombieEnded;
        _targets.TargetDied -= OnDied;
    }

    public void StartLevel()
    {
        GameStarted?.Invoke();
    }

    public void Continue()
    {
        Continued?.Invoke();
    }

    public void OnTryPause(InputAction.CallbackContext context)
    {
        if (_isGameOver)
            return;

        Paused?.Invoke();
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if(_isGameOver)
            Restart();
    }

    public void OnNextLevel(InputAction.CallbackContext context)
    {
        if (_isWin)
            Restart();
    }

    public void Restart()
    {
        DOTween.Clear(true);
        Restarted?.Invoke();
    }

    private void OnZombieEnded()
    {
        Win();
    }

    private void Win()
    {
        _isWin = true;
        Won?.Invoke();
    }

    private void OnDied()
    {
        Lose();
    }

    private void Lose()
    {
        _isGameOver = true;
        GameOver?.Invoke();
    }
}