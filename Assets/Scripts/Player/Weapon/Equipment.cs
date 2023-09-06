using InfimaGames.LowPolyShooterPack;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Equipment : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons = new List<Weapon>();

    private int _countOfInitedWeapons = 0;
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
    public event UnityAction<Weapon> Equiped;

    public void UpdateEquipment(Weapon weapon)
    {
        weapon.Equip();
        SetWeapon(weapon);
    }

    public List<Weapon> GetEquipedWeapons()
    {
        _equipmentWeapons = OrderEquipedWeaponsList();

        return _equipmentWeapons;
    }

    private void OnEnable()
    {
        for (int i = 0; i < _weapons.Count; i++)
            _weapons[i].Inited += OnInited;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _weapons.Count; i++)
            _weapons[i].Inited -= OnInited;
    }

    private void OnInited()
    {
        _countOfInitedWeapons++;

        if(_countOfInitedWeapons == _weapons.Count)
            InitWeapons(_weapons);
    }

    private List<Weapon> OrderEquipedWeaponsList()
    {
        List<Weapon> weapons = new List<Weapon>();

        if (_automaticRifle != null)
            weapons.Add(_automaticRifle);

        if (_pistol != null)
            weapons.Add(_pistol);

        if (_submachineGun != null)
            weapons.Add(_submachineGun);

        if (_shotgun != null)
            weapons.Add(_shotgun);

        if (_sniperRifle != null)
            weapons.Add(_sniperRifle);

        return weapons;
    }

    private void InitWeapons(List<Weapon> weapons)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].IsEquip == false)
                continue;

            SetWeapon(weapons[i]);
        }

        EquipByBoughtWeapon();

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

    private void ChangeEquipedWeapon(ref Weapon currentWeapon, Weapon newWeapon)
    {
        if (currentWeapon != null)
            currentWeapon.UnEquip();

        currentWeapon = newWeapon;
        AddToEquipmentList(currentWeapon);
        Equiped?.Invoke(currentWeapon);
    }

    private void AddToEquipmentList(Weapon weapon)
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

    private void EquipByBoughtWeapon()
    {
        if(IsWeaponTypeInList(_equipmentWeapons, WeaponType.AutomaticRifle) == false)
            EquipFistBoughtByTypeWeapon(WeaponType.AutomaticRifle);

        if (IsWeaponTypeInList(_equipmentWeapons, WeaponType.Pistol) == false)
            EquipFistBoughtByTypeWeapon(WeaponType.Pistol);

        if (IsWeaponTypeInList(_equipmentWeapons, WeaponType.SubmachineGun) == false)
            EquipFistBoughtByTypeWeapon(WeaponType.SubmachineGun);
    }

    private bool IsWeaponTypeInList(List<Weapon> weapons, WeaponType type)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Type == type)
                return true;
        }

        return false;
    }

    private void EquipFistBoughtByTypeWeapon(WeaponType type)
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            Weapon weapon = _weapons[i];

            if (weapon.Type == type && weapon.IsBought)
            {
                _equipmentWeapons.Add(weapon);
                weapon.Equip();
                Equiped?.Invoke(weapon);
                break;
            }
        }

        if (_automaticRifle == null)
            SetFirstAutomaticRifle();
    }

    private void SetFirstAutomaticRifle()
    {
        for (int i = 0; i < _equipmentWeapons.Count; i++)
        {
            if (_equipmentWeapons[i].Type == WeaponType.AutomaticRifle)
            {
                _automaticRifle = _equipmentWeapons[i];
                return;
            }
        }

        throw new ArgumentNullException(nameof(_automaticRifle));
    }

}