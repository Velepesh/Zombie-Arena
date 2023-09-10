using System.Threading.Tasks;
using UnityEngine;


public class CamerasEnabler : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _depthCamera;
    [SerializeField] private Camera _glowUICamera;
    [SerializeField] private float _delayBeforeEnableGlowCamera;

    private int _millisecindDelay => (int)_delayBeforeEnableGlowCamera * 1000;

    private void OnValidate()
    {
        _delayBeforeEnableGlowCamera = Mathf.Clamp(_delayBeforeEnableGlowCamera, 0, float.MaxValue);
    }

    private void OnEnable()
    {
        _game.GameStarted += OnGameStarted;
        _game.Won += OnWon;
        _game.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _game.GameStarted -= OnGameStarted;
        _game.Won -= OnWon;
        _game.GameOver -= OnGameOver;
    }

    private void Start()
    {
        EnableMainCamera();
    }

    private void EnableMainCamera()
    {
        Disable(_depthCamera);
        Disable(_glowUICamera);
        Enable(_mainCamera);
    }

    private void OnGameStarted()
    {
        Enable(_depthCamera);
    }

    private void OnWon()
    {
        EnableGlowUICamera();
    }

    private void OnGameOver()
    {
        EnableGlowUICamera();
    }

    private async void EnableGlowUICamera()
    {
        Enable(_glowUICamera);
        await Task.Delay(_millisecindDelay);
     
        Disable(_mainCamera);
        Disable(_depthCamera);
    }

    private void Enable(Camera camera)
    {
        camera.enabled = true;
    }

    private void Disable(Camera camera)
    {
        camera.enabled = false;
    }
}