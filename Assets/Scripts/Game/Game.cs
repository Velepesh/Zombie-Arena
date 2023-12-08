using DG.Tweening;
using System;
using UnityEngine.InputSystem;

public class Game : MonoCache
{
    private int _currentLevel;
    private bool _isPaused;
    private bool _isLose;
    private bool _isWin;
    private bool _canReborn = true;
    private GameMode _gameMode;

    public int CurrentLevel => _currentLevel;
    public bool IsLose => _isLose;
    public bool IsWon => _isWin;

    public event Action Inited;
    public event Action Won;
    public event Action Started;
    public event Action Ended;
    public event Action Unpaused;
    public event Action Paused;
    public event Action Restarted;
    public event Action Continued;
    public event Action GameOver;
    public event Action FirstLoss;
    public event Action InfinityGameEnded;
    public event Action Reborned;

    public void Init(int currentLevel)
    {
        if (currentLevel <= 0)
            throw new ArgumentException(nameof(currentLevel));
        
        _currentLevel = currentLevel;
    }

    public void Unpause()
    {
        _isPaused = false;
        Unpaused?.Invoke();
    }

    public void NextLevel()
    {
        Restart();
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

    public void Reborn()
    {
        Reborned?.Invoke();
    }

    public void ContunueAfterReborn()
    {
        _isLose = false;
        Continued?.Invoke();
    }

    public void StartLevel(GameMode gameMode)
    {
        _gameMode = gameMode;
        Started?.Invoke();
    }

    public void Win()
    {
        _isWin = true;
        Won?.Invoke();
    }


    public void End()
    {
        if (_isLose == true)
            return;

        _isLose = true;
        Ended?.Invoke();
        
        OfferToReborn();

        if (_gameMode == GameMode.Infinite)
            InfinityGameEnded?.Invoke();
    }

    private void OfferToReborn()
    {
        if (_canReborn)
        {
            FirstLoss?.Invoke();
            _canReborn = false;
        }
        else
        {
            GameOver?.Invoke();
        }
    }
}