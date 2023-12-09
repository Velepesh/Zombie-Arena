using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Reward _reward;
    [SerializeField] private Button _continueButton;
    [SerializeField] private CanvasFade _canvasFade;
    [SerializeField] private RewardInfo _rewardInfo;

    private int _currentReward;

    private void OnEnable()
    {
        _reward.Rewarded += OnRewarded;
        _canvasFade.Showed += OnShowed;
        _continueButton.onClick.AddListener(OnContinueButton);
    }

    private void OnDisable()
    {
        _reward.Rewarded -= OnRewarded;
        _canvasFade.Showed -= OnShowed;
        _continueButton.onClick.RemoveListener(OnContinueButton);
    }

    private void OnRewarded(int reward)
    {
        _currentReward = reward;
    }

    private void OnShowed()
    {
        ShowRewardInfo(_currentReward);
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