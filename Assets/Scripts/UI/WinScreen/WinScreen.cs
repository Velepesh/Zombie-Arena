using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private Game _game;
    [SerializeField] private TMP_Text _doubleEarningsText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private LangYGAdditionalText _additionalText;

    private readonly string _rightBracket = "]";
    private readonly string _dollar = "$";

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnRewardVideo;
        _game.Won += OnWon;
        _continueButton.onClick.AddListener(OnContinueButton);
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnRewardVideo;
        _game.Won -= OnWon;
        _continueButton.onClick.RemoveListener(OnContinueButton);
    }

    private void OnWon()
    {
        SetRewardInfo();
        _game.Reward();
    }

    private void SetRewardInfo()
    {
        _doubleEarningsText.text = $"{_game.DoubleEarnings} {_dollar}";
        _additionalText.additionalText = $"{_game.TotalScore}{_dollar} {_rightBracket}";
    }

    private void OnContinueButton()
    {
        _game.NextLevel();
    }

    private void OnRewardVideo(int id)
    {
        if (id == _adID)
        {
            _game.Reward();
            MetricaSender.Reward("double_earnings");
        }
    }
}