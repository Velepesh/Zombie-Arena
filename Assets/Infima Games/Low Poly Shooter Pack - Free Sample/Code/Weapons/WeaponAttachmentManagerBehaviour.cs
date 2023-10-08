//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.Events;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon Attachment Manager Behaviour.
    /// </summary>
    public abstract class WeaponAttachmentManagerBehaviour : MonoBehaviour
    {
        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake(){}

        /// <summary>
        /// Start.
        /// </summary>
        protected virtual void Start(){}

        /// <summary>
        /// Update.
        /// </summary>
        protected virtual void Update(){}

        /// <summary>
        /// Late Update.
        /// </summary>
        protected virtual void LateUpdate(){}

        #endregion
        
        #region GETTERS

        /// <summary>
        /// Returns the _equipped scope.
        /// </summary>
        public abstract ScopeBehaviour GetEquippedScope();
        /// <summary>
        /// Returns the _equipped scope default.
        /// </summary>
        public abstract ScopeBehaviour GetEquippedScopeDefault();
        
        /// <summary>
        /// Returns the _equipped magazine.
        /// </summary>
        public abstract MagazineBehaviour GetEquippedMagazine();
        /// <summary>
        /// Returns the _equipped muzzle.
        /// </summary>
        public abstract MuzzleBehaviour GetEquippedMuzzle();
        
        /// <summary>
        /// Returns the _equipped laser.
        /// </summary>
        public abstract LaserBehaviour GetEquippedLaser();
        /// <summary>
        /// Returns the _equipped grip.
        /// </summary>
        public abstract GripBehaviour GetEquippedGrip();

        public event UnityAction AttachmentInited;

        public abstract void InitWeaponAttachments();

        //protected void OnInited()
        //{
        //    AttachmentInited?.Invoke();
        //}
        #endregion
    }
}