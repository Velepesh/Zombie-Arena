using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private CursorStates _cursor;
    [SerializeField] private CompositionOrder _compositionOrder;
    [SerializeField] private ZombieTargetsCompositeRoot _targets;

    public event UnityAction GameStarted;
    public event UnityAction GameOver;
    public event UnityAction Continued;
    public event UnityAction Paused;

    private bool _isGameOver;

    private void OnEnable()
    {
        _targets.TargetDied += OnDied;
    }

    private void OnDisable()
    {
        _targets.TargetDied -= OnDied;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void StartLevel()
    {
        Init();
        GameStarted?.Invoke();
    }

    public void Continue()
    {
        Continued?.Invoke();
        _cursor.LockCursor();
        StartTime();
    }

    public void OnTryPause(InputAction.CallbackContext context)
    {
        if (_isGameOver)
            return;

        _cursor.UnlockCursor();
        Paused?.Invoke();
        StopTime();
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if(_isGameOver)
            Restart();
    }

    public void Restart()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void EndGame()
    {
        _isGameOver = true;
        GameOver?.Invoke();
        StopTime();
    }

    private void StartTime()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    private void StopTime()
    {
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    private void OnDied(IDamageable damageable)
    {
        EndGame();
    }

    private void Init()
    {
        StartTime();
        _cursor.LockCursor();
        _compositionOrder.Compose();
    }
}