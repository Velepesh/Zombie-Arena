using InfimaGames.LowPolyShooterPack;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class ReceivingPurchase : MonoBehaviour
{
    [SerializeField] private List<PurchaseYG> _purchases;
    [SerializeField] private UnityEvent<Weapon> _onSuccessPurchased;
    [SerializeField] private UnityEvent _onFailedPurchased;

    private PurchaseYG _currentPurchaseYG;
    public event UnityAction PurchaseBought;

    private void OnEnable()
    {
        YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessEvent;
        YandexGame.PurchaseFailedEvent += OnFailedPurchased;
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccessEvent;
        YandexGame.PurchaseFailedEvent -= OnFailedPurchased;
    }

    public void SetCurrentPurchase(PurchaseYG purchase)
    {
        _currentPurchaseYG = purchase;
    }

    public void BuyPurchase()
    {
        _currentPurchaseYG.BuyPurchase();
    }

    private void OnPurchaseSuccessEvent(string id)
    {
        if (_currentPurchaseYG == null)
            _currentPurchaseYG = GetPurchases(id);

        _currentPurchaseYG.data.consumed = true;

        _onSuccessPurchased?.Invoke(_currentPurchaseYG.Weapon);
        PurchaseBought?.Invoke();
    }

    private void OnFailedPurchased(string id)
    {
        _onFailedPurchased?.Invoke();
    }

    private PurchaseYG GetPurchases(string id)
    {
        for (int i = 0; i < _purchases.Count; i++)
        {
            if (_purchases[i].data.id == id)
                return _purchases[i];
        }

        throw new ArgumentNullException(nameof(id));
    }
}
