using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Reward _reward;
    [SerializeField] private Button _continueButton;
    [SerializeField] private RewardInfo _rewardInfo;

    private void OnEnable()
    {
        _reward.Rewarded += OnRewarded;
        _continueButton.onClick.AddListener(OnContinueButton);
    }

    private void OnDisable()
    {
        _reward.Rewarded -= OnRewarded;
        _continueButton.onClick.RemoveListener(OnContinueButton);
    }

    private void OnRewarded(int reward)
    {
        ShowRewardInfo(reward);
    }

    private void ShowRewardInfo(int reward)
    {
        _rewardInfo.ShowInfo(reward);
    }

    private void OnContinueButton()
    {
        _game.NextLevel();
    }
}