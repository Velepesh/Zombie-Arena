using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private CursorStates _cursor;
    [SerializeField] private PlayerBuilder _playerBuilder;
    [SerializeField] private TowerBuilder _towerBuilder;
    [SerializeField] private ZombieTargets _targets;
    [SerializeField] private ZombieSpawner _zombieSpawner;

    public event UnityAction GameStarted;
    public event UnityAction GameOver;
    public event UnityAction Settings;
    public event UnityAction Controls;
    public event UnityAction Restarted;
    public event UnityAction Continued;
    public event UnityAction Paused;

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
        DeactivateBuildingObjects();
    }

    public void RestartGame()
    {
        Restarted?.Invoke();
    }

    public void StartLevel()
    {
        Init();
        GameStarted?.Invoke();
    }

    public void OpenSettings()
    {
        Settings?.Invoke();
    }

    public void OpenControls()
    {
        Controls?.Invoke();
    }

    public void Continue()
    {
        _cursor.LockCursor();
        Continued?.Invoke();
        StartTimeScale();
        AudioListener.pause = false;
    }

    public void Pause()
    {
        _cursor.UnlockCursor();
        AudioListener.pause = true;
        Paused?.Invoke();
        StopTimeScale();
    }

    private void EndGame()
    {
        GameOver?.Invoke();
        StopTimeScale();
    }

    private void StartTimeScale()
    {
        Time.timeScale = 1;
    }

    private void StopTimeScale()
    {
        Time.timeScale = 0;
    }

    private void OnDied(IDamageable damageable)
    {
        EndGame();
    }

    private void Init()
    {
        _cursor.LockCursor();
        _playerBuilder.Form();
        _towerBuilder.Form();
        _targets.Init(_playerBuilder.Player, _towerBuilder.Tower);
        _zombieSpawner.StartSpawn(_targets);
    }

    private void DeactivateBuildingObjects()
    {
        _playerBuilder.Deactivate();
        _towerBuilder.Deactivate();
    }
}