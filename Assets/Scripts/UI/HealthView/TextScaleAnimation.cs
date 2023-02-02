using TMPro;
using UnityEngine;
using System.Collections;

public class TextScaleAnimation : MonoBehaviour
{
    [SerializeField] private float _startFontSize;
    [SerializeField] private float _targetFontSize;
    [SerializeField] private float _duration;

    private void OnValidate()
    {
        _startFontSize = Mathf.Clamp(_startFontSize, 0f, float.MaxValue);
        _targetFontSize = Mathf.Clamp(_targetFontSize, 0f, float.MaxValue);
        _duration = Mathf.Clamp(_duration, 0f, float.MaxValue);
    }

    public void PlayScaleAnimation(TMP_Text text)
    {
        StartCoroutine(EnableText(text));
    }

    private IEnumerator EnableText(TMP_Text text)
    {
        text.fontSize = _startFontSize;

        while (text.fontSize < _targetFontSize)
        {
            text.fontSize += Time.deltaTime * (_duration * 2);

            if (text.fontSize >= _targetFontSize)
            {
                text.fontSize = _targetFontSize;
                break;
            }

            yield return null;
        }

        while (text.fontSize > _startFontSize)
        {
            text.fontSize -= Time.deltaTime * (_duration * 2);

            if (text.fontSize <= _startFontSize)
            {
                text.fontSize = _targetFontSize;
                break;
            }

            yield return null;
        }
    }

    private void IncreaseFontSize()
    {

    }

    private void DecreaseFontSize()
    {

    }
}