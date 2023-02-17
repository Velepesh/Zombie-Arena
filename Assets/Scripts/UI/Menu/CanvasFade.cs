using DG.Tweening;
using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _duration;

    private Tween _fadeAction;

    public void Show()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        KillCurrentFade();
        _fadeAction = _canvasGroup?.DOFade(1, _duration);
    }

    public void Hide()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        KillCurrentFade();
        _fadeAction = _canvasGroup?.DOFade(0, _duration);
    }

    private void KillCurrentFade()
    {
        _fadeAction?.Kill();
    }
}