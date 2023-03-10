using UnityEngine;

[RequireComponent(typeof(CanvasFade))]
[RequireComponent(typeof(CanvasGroup))]
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Camera _glowUICamera;
    private CanvasGroup _canvasGroup;
    private CanvasFade _canvasFade;

    private void Awake()
    {
        _glowUICamera.enabled = false;
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasFade = GetComponent<CanvasFade>();
    }

    private void OnEnable()
    {
        _canvasFade.Showed += OnShowed;
    }

    private void OnDisable()
    {
        _canvasFade.Showed -= OnShowed;
    }

    private void OnShowed()
    {
        _glowUICamera.enabled = true;
    }
}