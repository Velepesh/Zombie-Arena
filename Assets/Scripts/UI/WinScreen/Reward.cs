using System;
using UnityEngine;
using YG;

public class Reward : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private int _minReward = 50;

    private Wallet _wallet;
    private int _currentReward;

    public event Action<int> Rewarded;
    public event Action DoubleRewarded;

    private void OnValidate()
    {
        _minReward = Math.Clamp(_minReward, 1, int.MaxValue);
    }

    public void Init(Wallet wallet)
    {
        if (wallet == null)
            throw new ArgumentNullException(nameof(wallet));

        _wallet = wallet;
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnRewardVideo;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnRewardVideo;
    }

    public void GiveWinReward(int totalScore)
    {
        SetReward(totalScore);
    }

    public void GiveGameOverReward(GameMode gameMode, int totalScore)
    {
        int reward = CalculateLoseAward(gameMode, totalScore);
        SetReward(reward);
    }

    private int CalculateLoseAward(GameMode gameMode, int totalScore)
    {
        if (totalScore < _minReward)
            totalScore += _minReward;

        if (gameMode == GameMode.Classic)
            return totalScore / 2;
        
        return totalScore;    
    }

    private void SetReward(int reward)
    {
        _currentReward = reward;
        AddMoney();
        Rewarded?.Invoke(_currentReward);
    }

    private void OnRewardVideo(int id)
    {
        if (id == _adID)
        {
            AddMoney();
            DoubleRewarded?.Invoke();
            MetricaSender.Reward("double_earnings");
        }
    }

    private void AddMoney()
    {
        _wallet.AddMoney(_currentReward);
    }
}