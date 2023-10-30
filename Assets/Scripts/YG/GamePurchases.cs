using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class GamePurchases : MonoBehaviour
{
    [SerializeField] private UnityEvent<Weapon> OnSuccessPurchased;
    [SerializeField] private UnityEvent OnFailedPurchased;
    [SerializeField] private PurchaseYG _purchaseYG;

    private Weapon _currentWeapon;

    private void OnEnable()
    {
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
        YandexGame.PurchaseFailedEvent += FailedPurchased;
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= SuccessPurchased;
        YandexGame.PurchaseFailedEvent -= FailedPurchased;
    }

    public void SetCurrentWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        _purchaseYG.SetDataId(weapon.Id);
    }

    private void SuccessPurchased(string id)
    {
        OnSuccessPurchased?.Invoke(_currentWeapon);
    }

    private void FailedPurchased(string id)
    {
        OnFailedPurchased?.Invoke();
    }
}