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
        _zombieSpawner.StartSpawn(_targets);//������� ������ ������ ����
        //���������� ������
        //����� ���� ����� ������, �� ���
        //���� ����� ������, � ����� �� �����-�� ����������, �� �����
        //����� ���������� � Zombie
    }

    public void MainMenu()
    {
       // LevelMainMenu?.Invoke();
    }


    public void RestartGame()
    {
        Restarted?.Invoke();
        //_sceneChanger.LoadLevel(CurrentLevel);
    }

    public void StartLevel()
    {
        GameStarted?.Invoke();
    }

    public void EndGame()
    {
        GameOver?.Invoke();
    }

    public void Pause()
    {
        Paused?.Invoke();
    }

    private void OnDied(IDamageable damageable)
    {
        EndGame();
    }
}
