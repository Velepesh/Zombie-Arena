using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class InventoryView : ItemView
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private CanvasGroup _inventoryViewCanvasGroup;

    public WeaponType Type => _weaponType;

    private void Awake()
    {
        ShowLockView();
    }

    public void UpdateView(Weapon weapon, int keyNumber)
    {
        if (weapon.IsEquip)
            ShowWeaponView(weapon, keyNumber);
        else
            ShowLockView();
    }

    private void ShowWeaponView(Weapon weapon, int keyNumber)
    {
        SetImages(weapon);
        SetKeyNumber(keyNumber);
        EnableCanvasGroup(_inventoryViewCanvasGroup);
    }

    private void ShowLockView()
    {
        DisableCanvasGroup(_inventoryViewCanvasGroup);
    }

    private void SetKeyNumber(int keyNumber)
    {
        Label.text = (keyNumber + 1).ToString();
    }

    private void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
    }

    private void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
    }
}