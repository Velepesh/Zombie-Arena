using UnityEngine;

public class CursorStates : MonoBehaviour
{
    public bool CursorLocked { get; private set; }

    private void Awake()
    {
        UnlockCursor();
    }

    public void LockCursor()
    {
        CursorLocked = true;
        UpdateCursorState();
    }

    public void UnlockCursor()
    {
        CursorLocked = false;
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        Cursor.visible = !CursorLocked;
        Cursor.lockState = CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}