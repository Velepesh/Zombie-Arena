using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputMapChanger : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;

    public void EnableUIActionMap()
    {
        _input.currentActionMap.Disable();
        _input.actions.FindActionMap("UI").Enable();
    }

    public void EnablePlayerActionMap()
    {
        _input.currentActionMap.Disable();
        _input.actions.FindActionMap("Player").Enable();
    }

    public void EnablePlayerInput()
    {
        _input.enabled = true;
    }

    public void DisablePlayerInput()
    {
        _input.enabled = false;
    }
}