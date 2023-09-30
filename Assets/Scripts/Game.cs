using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    [SerializeField] private ZombieTargetsCompositeRoot _targets;
    [SerializeField] private ZombieSpawner _zombieSpawner;
    [SerializeField] private ScoreSetup _scoreSetup;

    private int _defaultEarnings => TotalScore;
    private bool _isPaused;
    public int TotalScore => _scoreSetup.Score.TotalScore;
    public int DoubleEarnings => TotalScore * 2;

    public event UnityAction Won;
    public event UnityAction GameStarted;
    public event UnityAction GameOver;
    public event UnityAction Continued;
    public event UnityAction Paused;
    public event UnityAction Restarted;
    public event UnityAction<int> Earned;

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
        _isPaused = false;
        Continued?.Invoke();
    }

    public void NextLevel()
    {
        EndLevel(_defaultEarnings);
    }

    public void NextLevelDoubleEarnings()
    {
        EndLevel(DoubleEarnings);
    }

    public void OnTryPause(InputAction.CallbackContext context)
    {
        if (_isPaused == false)
        {
            Paused?.Invoke();
            _isPaused = true;
        }
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

    private void EndLevel(int money)
    {
        Earned?.Invoke(money);
        Restart();
    }

    private void Win()
    {
        Won?.Invoke();
    }

    private void OnDied()
    {
        Lose();
    }

    private void Lose()
    {
        GameOver?.Invoke();
    }
}