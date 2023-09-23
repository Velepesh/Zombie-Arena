using UnityEngine;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using System;
using YG;

public class EquipmentSaver : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;

    public void Start()
    {
        if (YandexGame.SDKEnabled)
            Load();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += Load;

        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought += OnBought;
            weapon.Equiped += OnEquiped;
        }
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;

        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought -= OnBought;
            weapon.Equiped -= OnEquiped;
        }
    }

    private void Load()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            WeaponData weaponData = YandexGame.savesData.Weapons[_weapons[i].Label];

            if (weaponData == null)
                throw new ArgumentNullException(nameof(weaponData));

            _weapons[i].LoadStates(weaponData);
        }
    }

    private void Save()
    {
        YandexGame.SaveProgress();
    }


    private void OnBought(Weapon weapon)
    {
        WeaponData weaponData = CheckSavedWeaponData(weapon);

        weaponData.IsBought = weapon.IsBought;
        weaponData.IsUnlock = weapon.IsUnlock;
        Save();
    }

    private void OnEquiped(Weapon weapon)
    {
        if (weapon.IsBought == false)
            return;

        WeaponData weaponData = CheckSavedWeaponData(weapon);

        weaponData.IsEquip = weapon.IsEquip;

        Save();
    }

    private WeaponData CheckSavedWeaponData(Weapon weapon)
    {
        WeaponData weaponData = YandexGame.savesData.Weapons[weapon.Label];

        if (weaponData == null)
            throw new ArgumentNullException(nameof(weaponData));

        return weaponData;
    }
}