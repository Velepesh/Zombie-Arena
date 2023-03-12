using UnityEngine;

public class CursorStates : MonoBehaviour
{
    public bool IsCursorLocked { get; private set; }

    private void Awake()
    {
        UnlockCursor();
    }

    public void LockCursor()
    {
        IsCursorLocked = true;
        UpdateCursorState();
    }

    public void UnlockCursor()
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