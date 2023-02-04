using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Outline))]
public class MarkerLine : MonoBehaviour
{
    private Image _image;
    private Outline _outline;

    private void Start()
    {
        _image = GetComponent<Image>();
        _outline = GetComponent<Outline>();
        ResetLine();
    }

    public void ShowLine(float duration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_image.DOFade(255, duration / 4f));
        sequence.Insert(0, _outline.DOFade(255, duration / 4f));
        sequence.AppendInterval(duration / 4f);
        sequence.Append(_image.DOFade(0, duration / 2f));
        sequence.Insert(0, _outline.DOFade(0, duration / 2f));
    }

    private void ResetLine()
    {
        Color imageColor = _image.color;
        imageColor.a = 0;
        _image.color = imageColor;

        Color otlineColor = _image.color;
        otlineColor.a = 0;
        _outline.effectColor = otlineColor;
    }
}