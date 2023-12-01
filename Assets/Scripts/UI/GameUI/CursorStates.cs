using UnityEngine;

public class CursorStates : MonoBehaviour
{
    [SerializeField] private Game _game;

    public bool IsCursorLocked { get; private set; }

    private void OnEnable()
    {
        _game.Started += OnGameStarted;
        _game.Unpaused += OnContinued;
        _game.Paused += OnPaused;
        _game.Won += OnWon;
        _game.Ended += OnGameOver;
        _game.Continued += OnReborned;
    }

    private void OnDisable()
    {
        _game.Started -= OnGameStarted;
        _game.Unpaused -= OnContinued;
        _game.Paused -= OnPaused;
        _game.Won -= OnWon;
        _game.Ended -= OnGameOver;
        _game.Continued -= OnReborned;
    }

    private void Awake()
    {
        UnlockCursor();
    }

    private void OnGameStarted()
    {
        LockCursor();
    }

    private void OnContinued()
    {
        LockCursor();
    }

    private void OnPaused()
    {
        UnlockCursor();
    }

    private void OnWon()
    {
        UnlockCursor();
    }

    private void OnGameOver()
    {
        UnlockCursor();
    }

    private void OnReborned()
    {
        LockCursor();
    }

    private void LockCursor()
    {
        IsCursorLocked = true;
        UpdateCursorState();
    }

    private void UnlockCursor()
    {
        IsCursorLocked = false;
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        Cursor.visible = !IsCursorLocked;
        Cursor.lockState = IsCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}