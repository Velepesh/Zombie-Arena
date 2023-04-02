using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InfimaGames.LowPolyShooterPack
{
    public class Inventory : InventoryBehaviour
    {
        //[SerializeField] private Equipment _equipment;
        #region FIELDS

        /// <summary>
        /// Array of all weaponsBehaviour. These are gotten in the order that they are parented to this object.
        /// </summary>
        private List<Weapon> _allWeapons;
        private List<Weapon> _equipedWeapons;
        
        /// <summary>
        /// Currently _equipped WeaponBehaviour.
        /// </summary>
        private Weapon _equipped;
        /// <summary>
        /// Currently _equipped index.
        /// </summary>
        private int _equippedIndex = -1;

        #endregion

        #region METHODS

        public event UnityAction<List<Weapon>> Setted;

        public override void Init(List<Weapon> weapons)
        {
            _equipedWeapons = weapons;

            _allWeapons = GetComponentsInChildren<Weapon>(true).ToList();

            for (int i = 0; i < _allWeapons.Count; i++)
                _allWeapons[i].gameObject.SetActive(false);

            Equip(0);
            Setted?.Invoke(_equipedWeapons);
        }
        

        public override WeaponBehaviour Equip(int index)
        {
            //If we have no weaponsBehaviour, we can't really equip anything.
            if (_equipedWeapons == null)
                return _equipped;

            //The index needs to be within the array's bounds.
            if (index > _equipedWeapons.Count - 1)
                return _equipped;

            //No point in allowing equipping the already-_equipped weapon.
            if (_equippedIndex == index)
                return _equipped;

            //Disable the currently _equipped weapon, if we have one.
            if (_equipped != null)
                _equipped.gameObject.SetActive(false);

            //Update index.
            _equippedIndex = index;
            //Update _equipped.
            _equipped = _equipedWeapons[_equippedIndex];
            //Activate the newly-_equipped weapon.
            _equipped.gameObject.SetActive(true);

            //Return.
            return _equipped;
        }
        
        #endregion

        #region Getters

        public override int GetLastIndex()
        {
            //Get last index with wrap around.
            int newIndex = _equippedIndex - 1;
            if (newIndex < 0)
                newIndex = _equipedWeapons.Count - 1;

            //Return.
            return newIndex;
        }

        public override int GetNextIndex()
        {
            //Get next index with wrap around.
            int newIndex = _equippedIndex + 1;
            if (newIndex > _equipedWeapons.Count - 1)
                newIndex = 0;

            //Return.
            return newIndex;
        }

        //public void OnTrySetAutomaticRifle(InputAction.CallbackContext context)
        //{
        //    TrySetWeapon(WeaponType.AutomaticRifle);
        //}

        //public void OnTrySetPistolRifle(InputAction.CallbackContext context)
        //{
        //    TrySetWeapon(WeaponType.Pistol);
        //}

        //public void OnTrySetSubmachineGunRifle(InputAction.CallbackContext context)
        //{
        //    TrySetWeapon(WeaponType.SubmachineGun);
        //}

        //public void OnTrySetShotgunRifle(InputAction.CallbackContext context)
        //{
        //    TrySetWeapon(WeaponType.Shotgun);
        //}

        //public void OnTrySetSniperRifle(InputAction.CallbackContext context)
        //{
        //    TrySetWeapon(WeaponType.SniperRifle);
        //}

        public override WeaponBehaviour GetEquipped() => _equipped;
        public override int GetEquippedIndex() => _equippedIndex;

        //private void TrySetWeapon(WeaponType type)
        //{
        //    int index = GetWeaponIndexByType(type);

        //    if (index >= 0)
        //        Equip(index);
        //}

        public override int GetWeaponIndexByType(WeaponType type)
        {
            for (int i = 0; i < _equipedWeapons.Count; i++)
            {
                if (_equipedWeapons[i].Type == type)
                    return i;
            }

            return -1;
        }

        #endregion
    }
}