using InfimaGames.LowPolyShooterPack;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Equipment : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons = new List<Weapon>();

    private List<Weapon> _equipmentWeapons = new List<Weapon>();
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

    public IEnumerable<Weapon> EquipmentWeapons => _equipmentWeapons;

    public event UnityAction Inited;
    public event UnityAction<Weapon> Equipped;

    private void Start()
    {
        InitWeapons(_weapons);
    }

    public void UpdateEquipment(Weapon weapon)
    {
        SetWeapon(weapon);
    }

    public List<Weapon> GetEquipedWeapons()
    {
        return _equipmentWeapons;
    }

    private void InitWeapons(List<Weapon> weapons)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].IsEquip == false)
                continue;

            SetWeapon(weapons[i]);
        }

        Inited?.Invoke();
    }

    private void SetWeapon(Weapon weapon)
    {
        switch (weapon.Type)
        {
            case WeaponType.AutomaticRifle:
                ChangeEquipedWeapon(ref _automaticRifle, weapon);
                break;
            case WeaponType.Pistol:
                ChangeEquipedWeapon(ref _pistol, weapon);
                break;
            case WeaponType.SubmachineGun:
                ChangeEquipedWeapon(ref _submachineGun, weapon);
                break;
            case WeaponType.Shotgun:
                ChangeEquipedWeapon(ref _shotgun, weapon);
                break;
            case WeaponType.SniperRifle:
                ChangeEquipedWeapon(ref _sniperRifle, weapon);
                break;
            default:
                throw new ArgumentNullException(nameof(weapon.Type));
        }
    }

    private void ChangeEquipedWeapon(ref Weapon currentWeapon, Weapon weapon)
    {
        if (currentWeapon != null)
            currentWeapon.UnEquip();

        currentWeapon = weapon;
        currentWeapon.Equip();
        AddToEquipmenrList(currentWeapon);
        Equipped?.Invoke(currentWeapon);
    }

    private void AddToEquipmenrList(Weapon weapon)
    {
        for (int i = 0; i < _equipmentWeapons.Count; i++)
        {
            if (_equipmentWeapons[i].Type == weapon.Type)
            {
                _equipmentWeapons[i] = weapon;
                return;
            }
        }

        _equipmentWeapons.Add(weapon);
    }
}