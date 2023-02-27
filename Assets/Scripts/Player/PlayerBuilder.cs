using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilder : Builder
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerViewSetup _setup;
    [SerializeField] private Character _character;

    public Player Player => _player;

    public override void Form()
    {
        _input.enabled = true;
        _animator.enabled = true;
        _setup.enabled = true;
        _character.UnlockCursor();
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