using UnityEngine;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using System;

public class GameStats : MonoBehaviour 
{
    [SerializeField] private Game _game;
    [SerializeField] private WalletSetup _walletSetup;
    [SerializeField] private List<Weapon> _weapons;

    private IDataService _dataService;
    private SaveData _saveData;

    public void Awake()
    {
        _dataService = new JsonDataService();
        _saveData = Load();

        _walletSetup.Init(_saveData.Money);

        for (int i = 0; i < _weapons.Count; i++)
        {
            WeaponData weaponData = _saveData.Weapons[_weapons[i].Label];

            if (weaponData == null)
                throw new ArgumentNullException(nameof(weaponData));

            _weapons[i].LoadStates(weaponData);
        }
    }

    private void OnEnable()
    {
        _game.GameOver += OnGameOver;

        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought += OnBought;
            weapon.Equiped += OnEquiped;
        }
    }

    private void OnDisable()
    {
        _game.GameOver -= OnGameOver;

        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            weapon.Bought -= OnBought;
            weapon.Equiped -= OnEquiped;
        }
    }

    private void OnGameOver()
    {
        SetMoney();
        SaveData(_saveData);
    }

    private void SetMoney()
    {
        _saveData.Money = _walletSetup.Wallet.Money;
    }

    private void OnBought(Weapon weapon)
    {
        WeaponData weaponData = CheckSavedWeaponData(weapon);

        weaponData.IsBought = weapon.IsBought;
        weaponData.IsUnlock = weapon.IsUnlock;
        SetMoney();
        SaveData(_saveData);
    }

    private void OnEquiped(Weapon weapon)
    {
        if (weapon.IsBought == false)
            return;

        WeaponData weaponData = CheckSavedWeaponData(weapon);

        weaponData.IsEquip = weapon.IsEquip;

        SaveData(_saveData);
    }

    private WeaponData CheckSavedWeaponData(Weapon weapon)
    {
        WeaponData weaponData = _saveData.Weapons[weapon.Label];

        if (weaponData == null)
            throw new ArgumentNullException(nameof(weaponData));

        return weaponData;
    }

    public void SaveData(SaveData data)
    {
        _dataService.SaveData(data);
    }

    public SaveData Load()
    {
        return _dataService.LoadData();
    }
}