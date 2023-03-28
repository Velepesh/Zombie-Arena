using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Spec _spec;
    [SerializeField] private Equipment _equipment;
    
    private WeaponView[] _weaponViews;

    private void Awake()
    {
        _weaponViews = GetComponentsInChildren<WeaponView>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _weaponViews.Length; i++)
            _weaponViews[i].Clicked += OnWeaponViewClicked;

        _equipment.Inited += OnEquipmentInited;
        _spec.BuyButtonClicked += OnBuyButtonClicked;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _weaponViews.Length; i++)
            _weaponViews[i].Clicked -= OnWeaponViewClicked;

        _equipment.Inited -= OnEquipmentInited;
        _spec.BuyButtonClicked -= OnBuyButtonClicked;
    }

    private void OnEquipmentInited()
    {
        Weapon weapon = _equipment.AutomaticRifle;

        _spec.UpdateSpec(weapon);
        UpdateWeaponViewByType(weapon);
        InitCurrentWeaponView(weapon);
    }

    private void OnWeaponViewClicked(Weapon weapon, WeaponView view)
    {
        _spec.UpdateSpec(weapon);
        UpdateExceptSelectWeaponViews(view);
    }

    private void OnBuyButtonClicked(Weapon weapon)
    {
        _equipment.UpdateEquipment(weapon);
        UpdateWeaponViewByType(weapon);
    }

    private void InitCurrentWeaponView(Weapon weapon)
    {
        for (int i = 0; i < _weaponViews.Length; i++)
        {
            if (_weaponViews[i].Weapon.Type == weapon.Type)
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
}