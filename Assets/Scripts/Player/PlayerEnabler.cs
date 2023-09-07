using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEnabler : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _cameraDepth;
    [SerializeField] private Character _character;
    [SerializeField] private LowerWeapon _lowerWeapon;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        DisableCamera(_cameraDepth);
        Deactivate();
    }

    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
        _game.GameOver += OnGameOver;
        _game.Paused += OnPaused;
        _game.Continued += OnContinued;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnGameStarted;
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
        EnableCamera(_cameraDepth);
        Activate();
    }

    private void OnGameOver()
    {
        DisableCamera(_mainCamera);
        DisableCamera(_cameraDepth);
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

    private void EnableCamera(Camera camera)
    {
        camera.enabled = true;
    }

    private void DisableCamera(Camera camera)
    {
        camera.enabled = false;
    }
}