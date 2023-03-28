using InfimaGames.LowPolyShooterPack;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Spec : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private —haracteristic _damageCharacteristic;
    [SerializeField] private —haracteristic _rpmCharacteristic;
    [SerializeField] private —haracteristic _magazineCharacteristic;
    [SerializeField] private —haracteristic _hipAccuracyCharacteristic;
    [SerializeField] private —haracteristic _aimAccuracyCharacteristic;
    [SerializeField] private —haracteristic _mobilityCharacteristic;
    [SerializeField] private string _equipmentText;
    [SerializeField] private string _adsText;
    [SerializeField] private Button _buyButton;

    private Weapon _currentWeapon;

    public event UnityAction<Weapon> BuyButtonClicked;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
    }

    public void UpdateSpec(Weapon weapon)
    {
        if (_canvasGroup.alpha != 1f)
            EnablePanel();

        _currentWeapon = weapon;
        SetLabel(weapon.Lable);
        Update—haracteristics(weapon);
    }

    private void OnBuyButtonClick()
    {
        BuyButtonClicked?.Invoke(_currentWeapon);
    }

    private void EnablePanel()
    {
        _canvasGroup.alpha = 1f;
    }

    private void DisablePanel()
    {
        _canvasGroup.alpha = 0f;
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