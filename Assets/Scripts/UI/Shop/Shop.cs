using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [SerializeField] private WalletSetup _walletSetup;
    [SerializeField] private Spec _spec;
    [SerializeField] private List<HealthAdder> _healthAdders;
    [SerializeField] private Equipment _equipment;

    private WeaponView[] _weaponViews;
    private EquipmentView[] _equipmentViews;

    private void Awake()
    {
        _weaponViews = GetComponentsInChildren<WeaponView>();
        _equipmentViews = GetComponentsInChildren<EquipmentView>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _weaponViews.Length; i++)
            _weaponViews[i].Clicked += OnWeaponViewClicked;

        for (int i = 0; i < _healthAdders.Count; i++)
            _healthAdders[i].BuyHealthButtonClicked += OnBuyHealthButtonClicked;

        _equipment.Inited += OnEquipmenInited;
        _equipment.Equiped += OnEquiped;
        _spec.EquipButtonClicked += OnEquipButtonClicked;
        _spec.BuyButtonClicked += OnBuyButtonClicked;
        _spec.AdsButtonClicked += OnAdsButtonClicked;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _weaponViews.Length; i++)
            _weaponViews[i].Clicked -= OnWeaponViewClicked;

        for (int i = 0; i < _healthAdders.Count; i++)
            _healthAdders[i].BuyHealthButtonClicked -= OnBuyHealthButtonClicked;

        _equipment.Inited -= OnEquipmenInited;
        _equipment.Equiped -= OnEquiped;
        _spec.EquipButtonClicked -= OnEquipButtonClicked;
        _spec.BuyButtonClicked -= OnBuyButtonClicked;
        _spec.AdsButtonClicked -= OnAdsButtonClicked;
    }

    private void OnEquipmenInited()
    {
        Weapon weapon = _equipment.AutomaticRifle;

        ShowSpecPanel();
        _spec.UpdateSpec(weapon);
        SetCurrentWeaponView(weapon);
    }

    private void OnEquiped(Weapon weapon)
    {
        UpdateEquipmentView(weapon);
        UpdateWeaponViewByType(weapon);
    }

    private void OnWeaponViewClicked(Weapon weapon, WeaponView view)
    {
        ShowSpecPanel();
        _spec.UpdateSpec(weapon);
        UpdateExceptSelectWeaponViews(view);
    }

    private void ShowSpecPanel()
    {
        _spec.ShowSpecPanel();

        for (int i = 0; i < _healthAdders.Count; i++)
            _healthAdders[i].HideHealthAdderPanel();
    }

    private void OnEquipButtonClicked(Weapon weapon)
    {
        Equip(weapon);
    }

    private void OnBuyHealthButtonClicked(HealthAdder healthAdder, int price)
    {
        if (_walletSetup.Wallet.Money >= price)
        {
            healthAdder.AddHealth();
            _walletSetup.Wallet.RemoveMoney(price);
        }
    }

    private void OnBuyButtonClicked(Weapon weapon)
    {
        TryBuyWeapon(weapon);
    }

    private void OnAdsButtonClicked(Weapon weapon)
    {
        UnlockWeaponForOnTry(weapon);
    }

    private void UnlockWeaponForOnTry(Weapon weapon)
    {
        weapon.Unlock();
        Equip(weapon);
    }

    private void TryBuyWeapon(Weapon weapon)
    {
        if (_walletSetup.Wallet.Money >= weapon.Price)
        {
            weapon.Buy();
            _walletSetup.Wallet.RemoveMoney(weapon.Price);
            Equip(weapon);
        }
    }

    private void Equip(Weapon weapon)
    {
        _equipment.UpdateEquipment(weapon);
        UpdateWeaponViewByType(weapon);
    }

    private void SetCurrentWeaponView(Weapon weapon)
    {
        for (int i = 0; i < _weaponViews.Length; i++)
        {
            if (_weaponViews[i].Weapon == weapon)
            {
                _weaponViews[i].SelectView();
                break;
            }
        }
    }

    private void UpdateExceptSelectWeaponViews(WeaponView view)
    {
        for (int i = 0; i < _weaponViews.Length; i++)
        {
            if (_weaponViews[i] != view)
                _weaponViews[i].UpdateView();
        }
    }

    private void UpdateWeaponViewByType(Weapon weapon)
    {
        for (int i = 0; i < _weaponViews.Length; i++)
        {
            if (_weaponViews[i].Weapon.Type == weapon.Type)
                _weaponViews[i].UpdateView();
        }
    }

    private void UpdateEquipmentView(Weapon weapon)
    {
        for (int i = 0; i < _equipmentViews.Length; i++)
        {
            if (_equipmentViews[i].Type == weapon.Type)
            {
                _equipmentViews[i].SetWeaponView(weapon);
                break;
            }
        }
    }
}