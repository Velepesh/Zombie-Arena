using System.Collections;
using UnityEngine;

public class HudDamageScreen : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] private float _delayFade = 0.25f;
    [Range(0.01f, 5)] [SerializeField] private float _fadeSpeed = 0.4f;
    [Range(0.1f, 0.9f)] [SerializeField] private float _minAlpha = 0.4f;
    [SerializeField] private AnimationCurve _curveFade;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Coroutine _fadeRedScreenJob;
    private float _alpha = 0;
    private float _nextDelay = 0;

    public void UpdateDamageScreen(int startHealth, int health)
    {
        if(startHealth == health)
            return;
        _alpha = (startHealth - health) / 100;
        _alpha = Mathf.Clamp(_alpha, _minAlpha, 1);

        _nextDelay = Time.unscaledTime + _delayFade;

        if (_fadeRedScreenJob != null)
            StopCoroutine(_fadeRedScreenJob);

        _fadeRedScreenJob = StartCoroutine(FadeRedScreen());
    }


    private IEnumerator FadeRedScreen()
    {
        while (_canvasGroup.alpha != _alpha)
        {
            if (Time.unscaledTime > _nextDelay && _alpha > 0)
            {
                _alpha = Mathf.Lerp(_alpha, 0, Time.unscaledDeltaTime);
                _alpha = _curveFade.Evaluate(_alpha);
            }

            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _alpha, Time.unscaledDeltaTime * _fadeSpeed);

            yield return null;
        }
    }
}
