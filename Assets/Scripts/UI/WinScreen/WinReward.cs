using System;
using UnityEngine;
using YG;

public class WinReward : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private Game _game;

    private Wallet _wallet;
    public Game Game => _game;

    public event Action<Game> WinRewarded;
    public event Action AdRewarded;

    public void Init(Wallet wallet)
    {
        _wallet = wallet;    
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnRewardVideo;
        _game.Won += OnWon;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnRewardVideo;
        _game.Won -= OnWon;
    }

    private void OnWon()
    {
        _wallet.AddMoney(_game.TotalScore);
        WinRewarded?.Invoke(_game);
    }

    private void OnRewardVideo(int id)
    {
        if (id == _adID)
        {
            _wallet.AddMoney(_game.TotalScore);
            AdRewarded();
            MetricaSender.Reward("double_earnings");
        }
    }
}