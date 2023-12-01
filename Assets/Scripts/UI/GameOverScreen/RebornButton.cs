using UnityEngine;
using UnityEngine.UI;
using YG;

public class RebornButton : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private RebornSetup _reborn;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    private void Rewarded(int id)
    {
        if (id == _adID)
            Reborn();
    }

    private void Reborn()
    {
        MetricaSender.Reward("reborn");
        _reborn.Reborn();
    }
}