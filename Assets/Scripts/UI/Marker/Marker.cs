using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Marker : MonoBehaviour
{
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Vector3 _targetScale;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private MarkerLine[] _markLines;

    private void Start()
    {
        _markLines = GetComponentsInChildren<MarkerLine>();
        _canvasGroup = GetComponent<CanvasGroup>();
        
        if (_markLines.Length == 0)
            throw new ArgumentNullException();
        
        _rectTransform = GetComponent<RectTransform>();
        ResetMarker();
    }

    public void Show(float duration)
    {
        ScaleMark(duration);

        for (int i = 0; i < _markLines.Length; i++)
            _markLines[i].ShowLine(duration, _canvasGroup);
    }

    private void ScaleMark(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_rectTransform.DOScale(_targetScale, duration / 4f));
        sequence.AppendInterval(duration / 4f);
        sequence.Append(_rectTransform.DOScale(_startScale, (duration) / 2f));
    }

    private void ResetMarker()
    {
        for (int i = 0; i < _markLines.Length; i++)
            _markLines[i].ResetLine(_canvasGroup);

        _rectTransform.localScale = _startScale;
    }
}