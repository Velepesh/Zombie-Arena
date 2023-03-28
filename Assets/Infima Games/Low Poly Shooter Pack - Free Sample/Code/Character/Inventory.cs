//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Inventory : InventoryBehaviour
    {
        [SerializeField] private Equipment _equipment;
        #region FIELDS

        /// <summary>
        /// Array of all weaponsBehaviour. These are gotten in the order that they are parented to this object.
        /// </summary>
        private WeaponBehaviour[] weaponsBehaviour;
        
        /// <summary>
        /// Currently equipped WeaponBehaviour.
        /// </summary>
        private WeaponBehaviour equipped;
        /// <summary>
        /// Currently equipped index.
        /// </summary>
        private int equippedIndex = -1;

        #endregion
        
        #region METHODS
        
        public override void Init(int equippedAtStart = 0)
        {
            //Cache all weaponsBehaviour. Beware that weaponsBehaviour need to be parented to the object this component is on!
            weaponsBehaviour = GetComponentsInChildren<WeaponBehaviour>(true);
            Weapon[] weapons = GetComponentsInChildren<Weapon>(true);

            _equipment.Init(weapons);
            //Disable all weaponsBehaviour. This makes it easier for us to only activate the one we need.
            foreach (WeaponBehaviour weapon in weaponsBehaviour)
                weapon.gameObject.SetActive(false);

            //Equip.
            Equip(equippedAtStart);
        }
        

        public override WeaponBehaviour Equip(int index)
        {
            //If we have no weaponsBehaviour, we can't really equip anything.
            if (weaponsBehaviour == null)
                return equipped;
            
            //The index needs to be within the array's bounds.
            if (index > weaponsBehaviour.Length - 1)
                return equipped;

            //No point in allowing equipping the already-equipped weapon.
            if (equippedIndex == index)
                return equipped;
            
            //Disable the currently equipped weapon, if we have one.
            if (equipped != null)
                equipped.gameObject.SetActive(false);

            //Update index.
            equippedIndex = index;
            //Update equipped.
            equipped = weaponsBehaviour[equippedIndex];
            //Activate the newly-equipped weapon.
            equipped.gameObject.SetActive(true);

            //Return.
            return equipped;
        }
        
        #endregion

        #region Getters

        public override int GetLastIndex()
        {
            //Get last index with wrap around.
            int newIndex = equippedIndex - 1;
            if (newIndex < 0)
                newIndex = weaponsBehaviour.Length - 1;

            //Return.
            return newIndex;
        }

        public override int GetNextIndex()
        {
            //Get next index with wrap around.
            int newIndex = equippedIndex + 1;
            if (newIndex > weaponsBehaviour.Length - 1)
                newIndex = 0;

            //Return.
            return newIndex;
        }

        public override WeaponBehaviour GetEquipped() => equipped;
        public override int GetEquippedIndex() => equippedIndex;

        #endregion
    }
}