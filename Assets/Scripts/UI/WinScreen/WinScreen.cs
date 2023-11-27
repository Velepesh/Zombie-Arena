using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private WinReward _winReward;
    [SerializeField] private TMP_Text _doubleEarningsText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private LangYGAdditionalText _additionalText;

    private readonly string _rightBracket = "]";
    private readonly string _dollar = "$";

    private void OnEnable()
    {
        _winReward.WinRewarded += OnWinRewarded;
        _winReward.AdRewarded += OnAdRewarded;
        _continueButton.onClick.AddListener(OnContinueButton);
    }

    private void OnDisable()
    {
        _winReward.WinRewarded -= OnWinRewarded;
        _winReward.AdRewarded -= OnAdRewarded;
        _continueButton.onClick.RemoveListener(OnContinueButton);
    }

    private void OnWinRewarded(Game game)
    {
        ShowRewardInfo(game);
    }

    private void ShowRewardInfo(Game game)
    {
        _doubleEarningsText.text = $"{game.DoubleEarnings} {_dollar}";
        _additionalText.additionalText = $"{game.TotalScore}{_dollar} {_rightBracket}";
    }

    private void OnAdRewarded()
    {
        OnContinueButton();
    }

    private void OnContinueButton()
    {
        _winReward.Game.NextLevel();
    }
}