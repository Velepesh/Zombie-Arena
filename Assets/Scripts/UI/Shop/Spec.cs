using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spec : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _inEquipmentText;
    [SerializeField] private TMP_Text _openText;
    [SerializeField] private —haracteristic _damageCharacteristic;
    [SerializeField] private —haracteristic _rpmCharacteristic;
    [SerializeField] private —haracteristic _magazineCharacteristic;
    [SerializeField] private —haracteristic _hipAccuracyCharacteristic;
    [SerializeField] private —haracteristic _aimAccuracyCharacteristic;
    [SerializeField] private —haracteristic _mobilityCharacteristic;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _unlockByAdsButton;
    [SerializeField] private Button _equipButton;

    private Weapon _currentWeapon;

    public event UnityAction<Weapon> EquipButtonClicked;
    public event UnityAction<Weapon> BuyButtonClicked;
    public event UnityAction<Weapon> AdsButtonClicked;

    private void OnEnable()
    {
        _equipButton.onClick.AddListener(OnEquipButtonClick);
        _buyButton.onClick.AddListener(OnBuyButtonClick);
        _unlockByAdsButton.onClick.AddListener(OnAdsButtonClick);
    }

    private void OnDisable()
    {
        _equipButton.onClick.RemoveListener(OnEquipButtonClick);
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        _unlockByAdsButton.onClick.RemoveListener(OnAdsButtonClick);
    }

    public void UpdateSpec(Weapon weapon)
    {
        _currentWeapon = weapon;
        SetInventoryText(weapon);
        SetLabel(weapon.Label);
        SetPrice(weapon.Price);
        Update—haracteristics(weapon);
        UpdateButtonsVisibility(weapon);
    }

    private void UpdateButtonsVisibility(Weapon weapon)
    {
        if (weapon.IsBought || weapon.IsUnlock)
        {
            DisableButtonView(_buyButton);
            DisableButtonView(_unlockByAdsButton);

            if (weapon.IsEquip)
                DisableButtonView(_equipButton);
            else
                EnableButtonView(_equipButton);
        }
        else
        {
            EnableButtonView(_buyButton);
            EnableButtonView(_unlockByAdsButton);
            DisableButtonView(_equipButton);
        }
    }

    private void DisableButtonView(Button button)
    {
        button.gameObject.SetActive(false);
    }

    private void EnableButtonView(Button button)
    {
        button.gameObject.SetActive(true);
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

    private void OnBuyButtonClick()
    {
        BuyButtonClicked?.Invoke(_currentWeapon);
        SetInventoryText(_currentWeapon);
        UpdateButtonsVisibility(_currentWeapon);
    }

    private void OnAdsButtonClick()
    {
        AdsButtonClicked?.Invoke(_currentWeapon);
        SetInventoryText(_currentWeapon);
        UpdateButtonsVisibility(_currentWeapon);
    }

    private void SetLabel(string label)
    {
        _label.text = label;
    }

    private void SetPrice(int price)
    {
        _price.text = price.ToString();
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