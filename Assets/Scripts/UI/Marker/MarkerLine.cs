using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Outline))]
public class MarkerLine : MonoBehaviour
{
    //[SerializeField] private float _targetImageA;
    //[SerializeField] private float _targetOtlineA;

    //private Image _image;
    //private Outline _outline;

    private void OnValidate()
    {
        //_targetImageA = Mathf.Clamp(_targetImageA, 0f, 255f);
        //_targetOtlineA = Mathf.Clamp(_targetOtlineA, 0f, 255f);
    }

    private void Start()
    {
        //_image = GetComponent<Image>();
        //_outline = GetComponent<Outline>();
       // ResetLine();
    }

    public void ShowLine(float duration, CanvasGroup canvasGroup)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(1, duration / 4f));
        //sequence.Append(_image.DOFade(_targetImageA, duration / 4f));
        //sequence.Insert(0, _outline.DOFade(_targetOtlineA, duration / 4f));
        sequence.AppendInterval(duration / 4f);
        //sequence.Append(_image.DOFade(0, duration / 2f));
        sequence.Append(canvasGroup.DOFade(0, duration / 2f));
        //sequence.Insert(0, _outline.DOFade(0, duration / 2f));
    }

    public void ResetLine(CanvasGroup canvasGroup)
    {
        //Color _imageColor = _image.color;
        canvasGroup.alpha = 0;
       // _image.color = _imageColor;

        //Color otlineColor = _image.color;
        //otlineColor.a = 0;
        //_outline.effectColor = otlineColor;
    }
}