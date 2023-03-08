using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilder : Builder
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerViewSetup _setup;

    public Player Player => _player;

    public override void Form()
    {
        _input.enabled = true;
        _animator.enabled = true;
        _setup.enabled = true;
    }

    public override void Deactivate()
    {
        _input.enabled = false;
        _animator.enabled = false;
        _setup.enabled = false;
    }

    public override void AddHealth()
    {
        _player.Health.AddHealth(10);
    }
}