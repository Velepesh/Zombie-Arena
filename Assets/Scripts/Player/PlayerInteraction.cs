using InfimaGames.LowPolyShooterPack;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Character _character;
    [SerializeField] private LowerWeapon _lowerWeapon;
    [SerializeField] private PlayerInputMapChanger _actionMapChanger;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delayBeforeDeactivate;

    private int _millisecindDelay => (int)_delayBeforeDeactivate * 1000;

    private void OnValidate()
    {
        _delayBeforeDeactivate = Mathf.Clamp(_delayBeforeDeactivate, 0, float.MaxValue);
    }

    private void Awake()
    {
        _actionMapChanger.DisablePlayerInput();
        Deactivate();
    }

    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
        _game.Restarted += OnRestarted;
        _game.Won += OnWon;
        _game.GameOver += OnGameOver;
        _game.Paused += OnPaused;
        _game.Continued += OnContinued;
        _game.Reborned += OnReborned;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnGameStarted;
        _game.Restarted -= OnRestarted;
        _game.Won -= OnWon;
        _game.GameOver -= OnGameOver;
        _game.Paused -= OnPaused;
        _game.Continued -= OnContinued;
        _game.Reborned -= OnReborned;
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.started)
            _game.Restart();
    }

    private void OnGameStarted()
    {
        _actionMapChanger.EnablePlayerInput();
        Activate();
    }

    private async void OnWon()
    {
        _actionMapChanger.DisablePlayerInput();
        await Task.Delay(_millisecindDelay);
        Deactivate();
    }

    private void OnGameOver()
    {
        _actionMapChanger.DisablePlayerInput();
        _actionMapChanger.EnableUIActionMap();
        Deactivate();
    }

    private void OnRestarted()
    {
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

    private void OnReborned()
    {
        Activate();
    }

    private void Activate()
    {
        _actionMapChanger.EnablePlayerActionMap();
        _character.enabled = true;
        _lowerWeapon.enabled = true;
        _animator.enabled = true;
    }

    private void Deactivate()
    {
        _character.enabled = false;
        _lowerWeapon.enabled = false;
        _animator.enabled = false;
    }
}