using System.Collections;
using UnityEngine;

public class CamerasEnabler : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _depthCamera;
    [SerializeField] private Camera _glowUICamera;
    [SerializeField] private float _delayBeforeEnableGlowCamera;

    private void OnValidate()
    {
        _delayBeforeEnableGlowCamera = Mathf.Clamp(_delayBeforeEnableGlowCamera, 0, float.MaxValue);
    }

    private void OnEnable()
    {
        _game.Started += OnGameStarted;
        _game.Won += OnWon;
        _game.Ended += OnEnded;
        _game.Continued += OnReborned;
    }

    private void OnDisable()
    {
        _game.Started -= OnGameStarted;
        _game.Won -= OnWon;
        _game.Ended -= OnEnded;
        _game.Continued -= OnReborned;
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
        StartCoroutine(SwitchToGlowCamera());
    }

    private void OnEnded()
    {
        EnableGlowUICamera();
    }

    private void OnReborned()
    {
        EnableMainCamera();
        OnGameStarted();
    }

    private IEnumerator SwitchToGlowCamera()
    {
        Enable(_glowUICamera);
        yield return new WaitForSeconds(_delayBeforeEnableGlowCamera);

        Disable(_mainCamera);
        Disable(_depthCamera);
    }

    private void EnableGlowUICamera()
    {
        Enable(_glowUICamera);
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