using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEnabler : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Character _character;
    [SerializeField] private LowerWeapon _lowerWeapon;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        Deactivate();
    }

    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
        _game.Paused += OnPaused;
        _game.Continued += OnContinued;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnGameStarted;
        _game.Paused -= OnPaused;
        _game.Continued -= OnContinued;
    }

    public void OnTryDeactivate(InputAction.CallbackContext context)
    {
        Deactivate();
    }

    private void OnGameStarted()
    {
        Activate();
    }

    private void OnPaused()
    {
        Deactivate();
    }

    private void OnContinued()
    {
        Activate();
    }

    private void Activate()
    {
        _character.enabled = true;
        _lowerWeapon.enabled = true;
        _input.enabled = true;
        _animator.enabled = true;
    }

    private void Deactivate()
    {
        _character.enabled = false;
        _lowerWeapon.enabled = false;
        _input.enabled = false;
        _animator.enabled = false;
    }
}