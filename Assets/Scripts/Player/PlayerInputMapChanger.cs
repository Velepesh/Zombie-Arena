using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputMapChanger : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;

    public void EnableUIActionMap()
    {
        DisableActionMap();
        SetActionMap("UI");
    }

    public void EnablePlayerActionMap()
    {
        DisableActionMap();
        SetActionMap("Player");
    }

    public void SetNoneActionMap()
    {
        DisableActionMap();
        SetActionMap("None");
    }

    public void EnablePlayerInput()
    {
        _input.enabled = true;
    }

    public void DisablePlayerInput()
    {
        _input.enabled = false;
    }

    private void SetActionMap(string actionMapName)
    {
        _input.actions.FindActionMap(actionMapName).Enable();
    }

    private void DisableActionMap()
    {
        _input.actions.Disable();
    }
}