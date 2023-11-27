using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using System;
using YG;

public class EquipmentSaver
{
    private IReadOnlyList<Weapon> _weapons;

    public EquipmentSaver(IReadOnlyList<Weapon> weapons)
    {
        _weapons = weapons;

        Load();
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

    public void OnBought(Weapon weapon)
    {
        WeaponData weaponData = CheckSavedWeaponData(weapon);

        weaponData.IsBought = weapon.IsBought;
        weaponData.IsUnlock = weapon.IsUnlock;

        Save();
    }

    public void OnEquiped(Weapon weapon)
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