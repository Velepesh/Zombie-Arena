using InfimaGames.LowPolyShooterPack;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Equipment : MonoBehaviour
{
    private List<EquipmentView> _equipmentViews = new List<EquipmentView>();
    private Weapon _automaticRifle;
    private Weapon _pistol;
    private Weapon _submachineGun;
    private Weapon _shotgun;
    private Weapon _sniperRifle;

    public Weapon AutomaticRifle => _automaticRifle;
    public Weapon Pistol => _pistol;
    public Weapon SubmachineGun => _submachineGun;
    public Weapon Shotgun => _shotgun;
    public Weapon SniperRifle => _sniperRifle;

    public event UnityAction Inited;

    public void Init(Weapon[] weapons)
    {
        _equipmentViews = GetComponentsInChildren<EquipmentView>().ToList();

        InitWeapons(weapons);
        Inited?.Invoke();
    }

    public void UpdateEquipment(Weapon weapon)
    {
        EquipWeapon(weapon);
        UpdateViews(weapon);
    }

    private void InitWeapons(Weapon[] weapons)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].IsEquip == false)
                continue;

            EquipWeapon(weapons[i]);
            UpdateViews(weapons[i]);
        }
    }

    private void EquipWeapon(Weapon weapon)
    {
        switch (weapon.Type)
        {
            case WeaponType.AutomaticRifle:
                SetWeapon(ref _automaticRifle, weapon);
                break;
            case WeaponType.Pistol:
                SetWeapon(ref _pistol, weapon);
                break;
            case WeaponType.SubmachineGun:
                SetWeapon(ref _submachineGun, weapon);
                break;
            case WeaponType.Shotgun:
                SetWeapon(ref _shotgun, weapon);
                break;
            case WeaponType.SniperRifle:
                SetWeapon(ref _sniperRifle, weapon);
                break;
            default:
                throw new ArgumentNullException(nameof(weapon.Type));
        }
    }

    private void SetWeapon(ref Weapon currentWeapon, Weapon weapon)
    {
        if (currentWeapon != null)
            currentWeapon.UnEquip();

        currentWeapon = weapon;
        currentWeapon.Equip();
    }

    private void UpdateViews(Weapon weapon)
    {
        for (int i = 0; i < _equipmentViews.Count; i++)
        {
            if (_equipmentViews[i].Type == weapon.Type)
            {
                _equipmentViews[i].UpdateView(weapon);
                break;
            }
        }
    }
}