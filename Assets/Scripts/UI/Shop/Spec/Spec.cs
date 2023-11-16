using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class Spec : MonoBehaviour
{
    [SerializeField] private int _adID;
    [SerializeField] private CanvasFade _canvasFade;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _yan;
    [SerializeField] private TMP_Text _inEquipmentText;
    [SerializeField] private TMP_Text _openText;
    [SerializeField] private —haracteristic _damageCharacteristic;
    [SerializeField] private —haracteristic _rpmCharacteristic;
    [SerializeField] private —haracteristic _magazineCharacteristic;
    [SerializeField] private —haracteristic _hipAccuracyCharacteristic;
    [SerializeField] private —haracteristic _aimAccuracyCharacteristic;
    [SerializeField] private —haracteristic _mobilityCharacteristic;
    [SerializeField] private SpecButton _buyForMoneyButton;
    [SerializeField] private SpecButton _buyForYanButton;
    [SerializeField] private SpecButton _unlockByAdsButton;
    [SerializeField] private SpecButton _equipButton;
    [SerializeField] private ReceivingPurchase _receivingPurchase;

    private Weapon _currentWeapon;

    public event UnityAction<Weapon> EquipButtonClicked;
    public event UnityAction<Weapon> BuyButtonClicked;
    public event UnityAction<Weapon> AdsButtonClicked;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        _equipButton.Clicked += OnEquipButtonClick;
        _buyForMoneyButton.Clicked += OnBuyForMoneyButtonClick;
        _buyForYanButton.Clicked += OnBuyForYanButtonClick;
        _receivingPurchase.PurchaseBought += OnPurchaseBought;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
        _equipButton.Clicked -= OnEquipButtonClick;
        _buyForMoneyButton.Clicked -= OnBuyForMoneyButtonClick;
        _buyForYanButton.Clicked -= OnBuyForYanButtonClick;
        _receivingPurchase.PurchaseBought -= OnPurchaseBought;
    }

    public void UpdateSpec(Weapon weapon)
    {
        _currentWeapon = weapon;

        if (weapon.TryGetComponent(out PurchaseYG purchase))
        {
            _receivingPurchase.SetCurrentPurchase(purchase);
            SetYan(purchase.data.priceValue);
        }

        SetInventoryText(weapon);
        SetLabel(weapon.Label);
        SetPrice(weapon.Price);
        Update—haracteristics(weapon);
        UpdateButtonsVisibility(weapon);
    }

    public void ShowSpecPanel()
    {
        if(_canvasGroup.alpha != 1)
            _canvasFade.Show();
    }

    private void UpdateButtonsVisibility(Weapon weapon)
    {
        if (weapon.IsBought || weapon.IsUnlock)
        {
            DisableButtonView(_buyForMoneyButton);
            DisableButtonView(_buyForYanButton);
            DisableButtonView(_unlockByAdsButton);

            if (weapon.IsEquip)
                DisableButtonView(_equipButton);
            else
                EnableButtonView(_equipButton);
        }
        else
        {
            EnableButtonView(_buyForMoneyButton);
            EnableButtonView(_buyForYanButton);
            EnableButtonView(_unlockByAdsButton);
            DisableButtonView(_equipButton);
        }
    }

    private void DisableButtonView(SpecButton specButton)
    {
        specButton.DisableButton();
    }

    private void EnableButtonView(SpecButton specButton)
    {
        specButton.EnableButton();
    }

    private void SetInventoryText(Weapon weapon)
    {
        DisableText(_inEquipmentText);
        DisableText(_openText);

        if (weapon.IsBought || weapon.IsUnlock)
        {
            if (weapon.IsEquip)
                EnableText(_inEquipmentText);
            else
                EnableText(_openText);
        }
    }

    private void EnableText(TMP_Text text)
    {
        text.enabled = true;
    }

    private void DisableText(TMP_Text text)
    {
        text.enabled = false;
    }

    private void OnEquipButtonClick()
    {
        EquipButtonClicked?.Invoke(_currentWeapon);
        SetInventoryText(_currentWeapon);
        UpdateButtonsVisibility(_currentWeapon);
    }

    private void OnBuyForMoneyButtonClick()
    {
        BuyButtonClicked?.Invoke(_currentWeapon);
        SetInventoryText(_currentWeapon);
        UpdateButtonsVisibility(_currentWeapon);
    }

    private void OnBuyForYanButtonClick()
    {
        _receivingPurchase.BuyPurchase();
    }

    private void OnPurchaseBought()
    {
        SetInventoryText(_currentWeapon);
        UpdateButtonsVisibility(_currentWeapon);
    }

    private void Rewarded(int id)
    {
        if (id == _adID)
        {
            AdsButtonClicked?.Invoke(_currentWeapon);
            SetInventoryText(_currentWeapon);
            UpdateButtonsVisibility(_currentWeapon);
            MetricaSender.Reward("weapon");
        }
    }

    private void SetLabel(string label)
    {
        _label.text = label;
    }

    private void SetPrice(int price)
    {
        _price.text = price.ToString();
    }

    private void SetYan(string yan)
    {
        _yan.text = yan;
    }

    private void Update—haracteristics(Weapon weapon)
    {
        _damageCharacteristic.Update—haracteristic(weapon.Damage);
        _rpmCharacteristic.Update—haracteristic(weapon.RoundsPerMinutes);
        _magazineCharacteristic.Update—haracteristic(weapon.GetAmmunitionTotal());
        _hipAccuracyCharacteristic.Update—haracteristic(weapon.HipSpread);
        _aimAccuracyCharacteristic.Update—haracteristic(weapon.AimSpread);
        _mobilityCharacteristic.Update—haracteristic(weapon.Mobility);
    }
}