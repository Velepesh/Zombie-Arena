using InfimaGames.LowPolyShooterPack;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetuper : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Character _character;
    [SerializeField] private LowerWeapon _lowerWeapon;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delayBeforeDeactivate;

    private int _millisecindDelay => (int)_delayBeforeDeactivate * 1000;

    private void OnValidate()
    {
        _delayBeforeDeactivate = Mathf.Clamp(_delayBeforeDeactivate, 0, float.MaxValue);
    }

    private void Awake()
    {
        Deactivate();
    }

    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
        _game.Won += OnWon;
        _game.GameOver += OnGameOver;
        _game.Paused += OnPaused;
        _game.Continued += OnContinued;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnGameStarted;
        _game.Won -= OnWon;
        _game.GameOver -= OnGameOver;
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

    private async void OnWon()
    {
        await Task.Delay(_millisecindDelay);
        Deactivate();
    }

    private async void OnGameOver()
    {
        await Task.Delay(_millisecindDelay);
        Deactivate();
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