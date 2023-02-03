using TMPro;
using UnityEngine;
using System.Collections;

public class TextScaleAnimation : MonoBehaviour
{
    [SerializeField] private float _startFontSize;
    [SerializeField] private float _targetFontSize;
    [SerializeField] private float _speed;

    private void OnValidate()
    {
        _startFontSize = Mathf.Clamp(_startFontSize, 0f, float.MaxValue);
        _targetFontSize = Mathf.Clamp(_targetFontSize, 0f, float.MaxValue);
        _speed = Mathf.Clamp(_speed, 0f, float.MaxValue);
    }

    public void PlayScaleAnimation(TMP_Text text)
    {
        StartCoroutine(IncreaseFontSize(text));
    }

    private IEnumerator IncreaseFontSize(TMP_Text text)
    {
        text.fontSize = _startFontSize;

        while (text.fontSize < _targetFontSize)
        {
            text.fontSize += Time.deltaTime * _speed;

            if (text.fontSize >= _targetFontSize)
            {
                text.fontSize = _targetFontSize;
                StartCoroutine(DecreaseFontSize(text));
                break;
            }

            yield return null;
        }
    }

    private IEnumerator DecreaseFontSize(TMP_Text text)
    {
        while (text.fontSize > _startFontSize)
        {
            text.fontSize -= Time.deltaTime * _speed;

            if (text.fontSize <= _startFontSize)
            {
                text.fontSize = _startFontSize;
                break;
            }

            yield return null;
        }
    }
}