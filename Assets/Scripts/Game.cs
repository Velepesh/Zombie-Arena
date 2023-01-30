using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class Game : MonoBehaviour
{
    //[SerializeField] private SceneChanger _sceneChanger;
    [SerializeField] private Player _player;
    [SerializeField] private Tower _tower;
    [SerializeField] private ZombieSpawner _zombieSpawner;

    private const string CURRENT_LEVEL_ID = "CurrentLevelID";

    private bool _isPlaying = false;
    public int CurrentLevel => PlayerPrefs.GetInt(CURRENT_LEVEL_ID, 1);

    public event UnityAction LevelStarted;
    public event UnityAction LevelWon;
    public event UnityAction LevelLost;
    public event UnityAction LevelRestart;
    public event UnityAction LevelMainMenu;

    private void OnEnable()
    {
        _player.Died += OnDied;
        _tower.Died += OnDied;
    }

    private void OnDisable()
    {
        _player.Died -= OnDied;
        _tower.Died -= OnDied;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        _zombieSpawner.StartSpawn(_player);//—делать логику поиска цели
        //изначально рандом
        //ѕотом если игрок близко, то его
        //≈сли башн€ близко, а игрок на каком-то рассто€нии, то башню
        //—мена происходит в Zombie
    }

    public void MainMenu()
    {
        LevelMainMenu?.Invoke();
    }

    public void NextLevel()
    {
        //_sceneChanger.LoadLevel(CurrentLevel);
    }

    public void RestartLevel()
    {
        LevelRestart?.Invoke();
        //_sceneChanger.LoadLevel(CurrentLevel);
    }

    public void StartLevel()
    {
        _isPlaying = true;
        //_zombie.StartLevel();

        LevelStarted?.Invoke();
    }

    public void WinGame()
    {
        _isPlaying = false;
        LevelWon?.Invoke();
        PlayerPrefs.SetInt(CURRENT_LEVEL_ID, CurrentLevel + 1);
    }

    public void LoseGame()
    {
        _isPlaying = false;
        LevelLost?.Invoke();
    }

    private void OnDied(IDamageable damageable)
    {
        LoseGame();
    }
}
