using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using YG;

public class RebornButton : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private Game _game;
    [SerializeField] private Button _rebornButton;
    [SerializeField] private List<Builder> _builders;

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
        if(_builders.Count == 0)
           throw new ArgumentNullException(nameof(_builders));

        for (int i = 0; i < _builders.Count; i++)
            _builders[i].Reborn();

        _game.Reborn();
        DisableRebornButton();
    }

    private void DisableRebornButton()
    {
        _rebornButton.gameObject.SetActive(false);
    }
}