using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentView : ItemView
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private CanvasGroup _weaponViewCanvasGroup;
    [SerializeField] private CanvasGroup _emptySlotCanvasGroup;

    public WeaponType Type => _weaponType;

    public event UnityAction<Weapon, WeaponView> Clicked;

    private void Awake()
    {
        ShowEmptySlot();
    }

    public void UpdateView(Weapon weapon)
    {
        if (_weaponViewCanvasGroup.alpha != 1f)
            ShowWeaponView();

        SetImages(weapon);
        SetLabel(weapon);
    }

    private void ShowWeaponView()
    {
        DisableCanvasGroup(_emptySlotCanvasGroup);
        EnableCanvasGroup(_weaponViewCanvasGroup);
    }

    private void ShowEmptySlot()
    {
        DisableCanvasGroup(_weaponViewCanvasGroup);
        EnableCanvasGroup(_emptySlotCanvasGroup);
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