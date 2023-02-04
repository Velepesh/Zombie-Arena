using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Vector3 _targetScale;

    private RectTransform _rectTransform;
    private MarkerLine[] _markLines;

    private void Start()
    {
        _markLines = GetComponentsInChildren<MarkerLine>();
        
        if (_markLines.Length == 0)
            throw new ArgumentNullException();
        
        _rectTransform = GetComponent<RectTransform>();
        ResetSacle();
    }

    public void Show(float duration)
    {
        ScaleMark(duration);

        for (int i = 0; i < _markLines.Length; i++)
            _markLines[i].ShowLine(duration);
    }

    private void ScaleMark(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_rectTransform.DOScale(_targetScale, duration / 4f));
        sequence.AppendInterval(duration / 4f);
        sequence.Append(_rectTransform.DOScale(_startScale, (duration) / 2f));
    }

    private void ResetSacle()
    {
        _rectTransform.localScale = _startScale;
    }
}