using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private float _startAppeareDuration;
    [SerializeField] private float _waitBeforeMoveDuration;
    [SerializeField] private float _moveDuration;
    [SerializeField] private RectTransform _target;
    [SerializeField] private RectTransform _spawnPoint;
    [SerializeField] private TMP_Text _addedScoreText;
    [SerializeField] private TMP_Text _totalScoreText;
    [SerializeField] private Canvas _copyOfMainCanvas;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void OnValidate()
    {
        _startAppeareDuration = Mathf.Clamp(_startAppeareDuration, 0f, float.MaxValue);
        _waitBeforeMoveDuration = Mathf.Clamp(_waitBeforeMoveDuration, 0f, float.MaxValue);
        _moveDuration = Mathf.Clamp(_moveDuration, 0f, float.MaxValue);
    }

    public void SetScoreValue(int value)
    {
        _totalScoreText.text = value.ToString();
    }

    public void SetAmount(int addedScore, int totalScore)
    {
         StartCoroutine(AddScore(addedScore, totalScore));
    }

    private void PlayAnimation()
    {
        FadeText();
        MoveText();
    }

    private void MoveText()
    {
        float scaleFactor = _copyOfMainCanvas.scaleFactor;
        Vector2 finalPosition = new Vector2(_target.position.x / scaleFactor, _target.position.y / scaleFactor) ;
        _addedScoreText.rectTransform.DOAnchorPos(finalPosition, _moveDuration);
    }

    private void FadeText()
    {
        _canvasGroup.DOFade(0, _moveDuration);
    }

    private IEnumerator AddScore(int addedScore, int totalScore)
    {
        AppeareText(addedScore);
        yield return new WaitForSeconds(_waitBeforeMoveDuration);

        PlayAnimation();
        yield return new WaitForSeconds(_moveDuration);

        SetScoreValue(totalScore);
    }

    private void AppeareText(int score)
    {
        _addedScoreText.text = score.ToString();
        _addedScoreText.rectTransform.position = _spawnPoint.position;
        _canvasGroup.DOFade(1, _startAppeareDuration);
    }
}