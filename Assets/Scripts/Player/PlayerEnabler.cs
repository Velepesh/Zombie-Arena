using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEnabler : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;

    public void Activate()
    {
        _input.enabled = true;
        _animator.enabled = true;
    }

    public void Deactivate()
    {
        _input.enabled = false;
        _animator.enabled = false;
    }

    public void OnTryDeactivate(InputAction.CallbackContext context)
    {
        Deactivate();
    }
}