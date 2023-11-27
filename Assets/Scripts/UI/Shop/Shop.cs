using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [SerializeField] private WalletSetup _walletSetup;
    [SerializeField] private Spec _spec;
    [SerializeField] private List<HealthAdder> _healthAdders;
    [SerializeField] private List<Button> _healthAdderBonusButtons;
    
    private Equipment _equipment;
    private WeaponView[] _weaponViews;
    private EquipmentView[] _equipmentViews;

    private void OnDisable()
    {
        for (int i = 0; i < _weaponViews.Length; i++)
            _weaponViews[i].Clicked -= OnWeaponViewClicked;

        for (int i = 0; i < _healthAdders.Count; i++)
            _healthAdders[i].BuyHealthButtonClicked -= OnBuyHealthButtonClicked;

        for (int i = 0; i < _healthAdderBonusButtons.Count; i++)
            _healthAdderBonusButtons[i].onClick.RemoveListener(OnHealthAdderBonusButtons);

        if (_equipment != null)
        {
            _equipment.Inited -= OnEquipmentInited;
            _equipment.WeaponEquiped -= OnEquiped;
        }

        _spec.EquipButtonClicked -= OnEquipButtonClicked;
        _spec.BuyButtonClicked -= OnBuyButtonClicked;
        _spec.AdsButtonClicked -= OnAdsButtonClicked;
    }

    public void Init(Equipment equipment)
    {
        if(equipment == null)
            throw new ArgumentNullException(nameof(equipment));

        _equipment = equipment;
        
        _equipment.Inited += OnEquipmentInited;
        _equipment.WeaponEquiped += OnEquiped;
        _spec.EquipButtonClicked += OnEquipButtonClicked;
        _spec.BuyButtonClicked += OnBuyButtonClicked;
        _spec.AdsButtonClicked += OnAdsButtonClicked;

        _weaponViews = GetComponentsInChildren<WeaponView>();
        _equipmentViews = GetComponentsInChildren<EquipmentView>();

        for (int i = 0; i < _weaponViews.Length; i++)
            _weaponViews[i].Clicked += OnWeaponViewClicked;

        for (int i = 0; i < _healthAdders.Count; i++)
            _healthAdders[i].BuyHealthButtonClicked += OnBuyHealthButtonClicked;

        for (int i = 0; i < _healthAdderBonusButtons.Count; i++)
            _healthAdderBonusButtons[i].onClick.AddListener(OnHealthAdderBonusButtons);
    }

    public void BuyWeaponForYan(Weapon weapon)
    {
        weapon.Buy();
        Equip(weapon);
        MetricaSender.Yan(weapon.Label);
    }

    private void OnEquipmentInited()
    {
        Weapon weapon = _equipment.AutomaticRifle;

        ShowSpecPanel();
        _spec.UpdateSpec(weapon);
        SelectWeaponView(weapon);
    }

    private void OnHealthAdderBonusButtons()
    {
        for (int i = 0; i < _weaponViews.Length; i++)
            _weaponViews[i].UpdateView();
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
            MetricaSender.Money("health_adder", healthAdder.Label, price);
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
            MetricaSender.Money("weapon", weapon.Label, weapon.Price);
        }
    }

    private void Equip(Weapon weapon)
    {
        _equipment.UpdateEquipment(weapon);
        UpdateWeaponViewByType(weapon);
    }

    private void SelectWeaponView(Weapon weapon)
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