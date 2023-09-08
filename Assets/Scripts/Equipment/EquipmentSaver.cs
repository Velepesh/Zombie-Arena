using UnityEngine;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using System;

public class EquipmentSaver : MonoBehaviour , ISaver
{
    [SerializeField] private List<Weapon> _weapons;

    readonly string _path = "/eqipment.json";

    private IDataService _dataService;
    private EquipmentData _data;

    public void Awake()
    {
        _dataService = new JsonDataService();
        LoadData();

        for (int i = 0; i < _weapons.Count; i++)
        {
            WeaponData weaponData = _data.Weapons[_weapons[i].Label];

            if (weaponData == null)
                throw new ArgumentNullException(nameof(weaponData));

            _weapons[i].LoadStates(weaponData);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought += OnBought;
            weapon.Equiped += OnEquiped;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought -= OnBought;
            weapon.Equiped -= OnEquiped;
        }
    }

    private void OnBought(Weapon weapon)
    {
        WeaponData weaponData = CheckSavedWeaponData(weapon);

        weaponData.IsBought = weapon.IsBought;
        weaponData.IsUnlock = weapon.IsUnlock;
        SaveData(_path, _data);
    }

    private void OnEquiped(Weapon weapon)
    {
        if (weapon.IsBought == false)
            return;

        WeaponData weaponData = CheckSavedWeaponData(weapon);

        weaponData.IsEquip = weapon.IsEquip;

        SaveData(_path, _data);
    }

    private WeaponData CheckSavedWeaponData(Weapon weapon)
    {
        WeaponData weaponData = _data.Weapons[weapon.Label];

        if (weaponData == null)
            throw new ArgumentNullException(nameof(weaponData));

        return weaponData;
    }

    public void SaveData<EquipmentData>(string path ,EquipmentData data)
    {
        _dataService.SaveData(path, data);
    }

    public void LoadData()
    {
        _data = _dataService.LoadData<EquipmentData>(_path);

        if (_data == null)
            _data = new EquipmentData();
    }
}