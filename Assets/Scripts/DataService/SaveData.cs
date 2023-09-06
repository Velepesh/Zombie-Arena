using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int Level = 1;
    public int Money = 0;
    public Dictionary<string, WeaponData> Weapons = new Dictionary<string, WeaponData>()
    {
        { "AK-47", new WeaponData(){IsBought = true, IsEquip = true, IsUnlock = true}},
        { "M4A1", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "SCAR", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "Colt M1911", new WeaponData(){IsBought = true, IsEquip = true, IsUnlock = true}},
        { "Beretta 92", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "HK USP", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "Sniper Rifle", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "Shotgun", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "UZI", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "P90", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
        { "UMP 45", new WeaponData(){IsBought = false, IsEquip = false, IsUnlock = false}},
    };
}

[Serializable]
public class WeaponData
{
    public bool IsBought;
    public bool IsEquip;
    public bool IsUnlock;
}