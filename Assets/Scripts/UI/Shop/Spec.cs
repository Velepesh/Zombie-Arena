using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spec : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _inventoryText;
    [SerializeField] private —haracteristic _damageCharacteristic;
    [SerializeField] private —haracteristic _rpmCharacteristic;
    [SerializeField] private —haracteristic _magazineCharacteristic;
    [SerializeField] private —haracteristic _hipAccuracyCharacteristic;
    [SerializeField] private —haracteristic _aimAccuracyCharacteristic;
    [SerializeField] private —haracteristic _mobilityCharacteristic;
    [SerializeField] private string _equipmentText;
    [SerializeField] private string _adsText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _unlockByAdsButton;

    private Weapon _currentWeapon;

    public event UnityAction<Weapon> BuyButtonClicked;
    public event UnityAction<Weapon> AdsButtonClicked;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClick);
        _unlockByAdsButton.onClick.AddListener(OnAdsButtonClick);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        _unlockByAdsButton.onClick.RemoveListener(OnAdsButtonClick);
    }

    public void UpdateSpec(Weapon weapon)
    {
        _currentWeapon = weapon;
        SetInventoryText(weapon);
        SetLabel(weapon.Lable);
        Update—haracteristics(weapon);
    }

    private void SetInventoryText(Weapon weapon)
    {
        if (weapon.IsEquip)
            _inventoryText.text = _equipmentText;
        else
            _inventoryText.text = _adsText;
    }

    private void OnBuyButtonClick()
    {
        BuyButtonClicked?.Invoke(_currentWeapon);
        SetInventoryText(_currentWeapon);
    }

    private void OnAdsButtonClick()
    {
        //AdsButtonClicked?.Invoke(_currentWeapon);
        //SetInventoryText(_currentWeapon);
    }


    private void SetLabel(string label)
    {
        _label.text = label;
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