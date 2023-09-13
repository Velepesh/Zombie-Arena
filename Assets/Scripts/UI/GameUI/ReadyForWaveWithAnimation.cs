using DG.Tweening;
using TMPro;
using UnityEngine;
using YG;

[RequireComponent(typeof(ZombieSpawnerView))]
public class ReadyForWaveWithAnimation : MonoBehaviour
{
    [SerializeField] private Color _waveNumberColor;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _showDuration;
    [SerializeField] private TMP_Text _waveNumberText;
    [SerializeField] private TMP_Text _getReadyForNextWaveText;
    [SerializeField] private CanvasGroup _canvasGroup;

    private ZombieSpawnerView _view;

    private void Awake()
    {
        _view = GetComponent<ZombieSpawnerView>();
    }

    private void OnEnable()
    {
        _view.WaveSetted += OnWaveSetted;
    }

    private void OnDisable()
    {
        _view.WaveSetted -= OnWaveSetted;
    }

    public void OnWaveSetted(int currentWave)
    {
        _waveNumberText.text = $"{currentWave}";
        
        ShowNextWaveInformation();
    }

    private void ShowNextWaveInformation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(1, _fadeDuration));
        sequence.AppendInterval(_showDuration);
        sequence.Append(_canvasGroup.DOFade(0, _fadeDuration));
    }
}