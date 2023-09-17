using UnityEngine;

public class CursorStates : MonoBehaviour
{
    [SerializeField] private Game _game;

    public bool IsCursorLocked { get; private set; }


    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
        _game.Continued += OnContinued;
        _game.Paused += OnPaused;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnGameStarted;
        _game.Continued -= OnContinued;
        _game.Paused -= OnPaused;
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