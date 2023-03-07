using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerBuilder _playerBuilder;
    [SerializeField] private TowerBuilder _towerBuilder;
    [SerializeField] private ZombieTargets _targets;
    [SerializeField] private ZombieSpawner _zombieSpawner;

    public event UnityAction GameStarted;
    public event UnityAction GameOver;
    public event UnityAction Restarted;
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

    public void EndGame()
    {
        GameOver?.Invoke();
        Time.timeScale = 0;
    }

    public void Pause()
    {
        Paused?.Invoke();
    }

    private void OnDied(IDamageable damageable)
    {
        EndGame();
    }

    private void Init()
    {
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
