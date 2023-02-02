using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderAnimation : MonoBehaviour
{
    [SerializeField] private ZombieHealthView _view;
    [SerializeField] private Image _background;
    [SerializeField] private Image _fillArea;
    [SerializeField] private float _appeareDuration;
    [SerializeField] private float _fadeDuration;

    private void OnValidate()
    {
        _appeareDuration = Mathf.Clamp(_appeareDuration, 0f, float.MaxValue);
        _fadeDuration = Mathf.Clamp(_fadeDuration, 0f, float.MaxValue);
    }

    private void OnEnable()
    {
        _view.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _view.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        ShowAnimation();
    }

    private void ShowAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_background.DOFade(255, _appeareDuration));
        sequence.Insert(0, _fillArea.DOFade(255, _appeareDuration));

        sequence.Append(_background.DOFade(0, _fadeDuration));
        sequence.Insert(_appeareDuration, _fillArea.DOFade(0, _fadeDuration));
    }
}