using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
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

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void MainMenu()
    {
       // LevelMainMenu?.Invoke();
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

    private void Init()//Пробросить начало, что игрок начинает с анимации на H
    {
        _targets.Init();
        _zombieSpawner.StartSpawn(_targets);
    }
}
