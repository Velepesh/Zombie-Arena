//Copyright 2022, Infima Games. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Abstract Inventory Class. Helpful so you can implement your own inventory system!
    /// </summary>
    public abstract class InventoryBehaviour : MonoBehaviour
    {
        #region GETTERS

        /// <summary>
        /// Returns the index that is before the current index. Very helpful in order to figure out
        /// what the next weapon to equip is.
        /// </summary>
        /// <returns></returns>
        public abstract int GetLastIndex();
        /// <summary>
        /// Returns the next index after the currently _equipped one. Very helpful in order to figure out
        /// what the next weapon to equip is.
        /// </summary>
        public abstract int GetNextIndex();
        /// <summary>
        /// Returns the currently _equipped WeaponBehaviour.
        /// </summary>
        public abstract WeaponBehaviour GetEquipped();

        /// <summary>
        /// Returns the currently _equipped index. Meaning the index in the weapon array of the _equipped weapon.
        /// </summary>
        public abstract int GetEquippedIndex();

        public abstract int GetWeaponIndexByType(WeaponType type);
        #endregion

        #region METHODS

        /// <summary>
        /// SpawnPrefab. This function is called when the game starts. We don't use Awake or Start because we need the
        /// PlayerCharacter component to run this with the index it wants to equip!
        /// </summary>
        /// <param name="equippedAtStart">Inventory index of the weapon we want to equip when the game starts.</param>
        public abstract void Init(List<Weapon> weapons);
        
        /// <summary>
        /// Equips a Weapon.
        /// </summary>
        /// <param name="index">Index of the weapon to equip.</param>
        /// <returns>Weapon that was just _equipped.</returns>
        public abstract WeaponBehaviour Equip(int index);

        #endregion
    }
}