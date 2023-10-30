//Copyright 2022, Infima Games. All Rights Reserved.

using InfimaGames.LowPolyShooterPack.Legacy;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon. This class handles most of the things that weaponsBehaviour need.
    /// </summary>
    public class Weapon : WeaponBehaviour
    {
        [SerializeField] private WeaponType _type;
        [SerializeField] private bool _isBought;
        [SerializeField] private bool _isUnlock;
        [SerializeField] private bool _isEquip;
        [SerializeField] private int _price;
        [SerializeField] private int _yan;
        [SerializeField] private string _id;
        #region FIELDS SERIALIZED

        [Title(label: "Settings")]

        [SerializeField] private ProjectilePool _bulletPool;
        [SerializeField] private CasingPool _casingPool;
        [SerializeField] private AudioMixerGroup _mixerGroup;

        [Tooltip("Weapon Name. Currently not used for anything, but in the future, we will use this for pickups!")]
        [SerializeField] 
        private string weaponName;

        [Tooltip("How much the character's movement speed is multiplied by when wielding this weapon.")]
        [SerializeField]
        private float multiplierMovementSpeed = 1.0f;
        
        [Title(label: "Firing")]

        [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
        [SerializeField] 
        private bool automatic;
        
        [Tooltip("Is this weapon bolt-action? If yes, then a bolt-action animation will play after every shot.")]
        [SerializeField]
        private bool boltAction;

        [Tooltip("Amount of shots fired at once. Helpful for things like shotguns, where there are multiple projectiles fired at once.")]
        [SerializeField]
        private int shotCount = 1;
        
        [Tooltip("How far the weapon can fire from the center of the screen.")]
        [SerializeField]
        private float spread = 0.25f;
        
        [Tooltip("How fast the projectiles are.")]
        [SerializeField]
        private float projectileImpulse = 400.0f;

        [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
        [SerializeField] 
        private int roundsPerMinutes = 200;

        [Title(label: "Reloading")]
        
        [Tooltip("Determines if this weapon reloads in cycles, meaning that it inserts one bullet at a time, or not.")]
        [SerializeField]
        private bool cycledReload;
        
        [Tooltip("Determines if the player can reload this weapon when it is full of ammunition.")]
        [SerializeField]
        private bool canReloadWhenFull = true;

        [Tooltip("Should this weapon be reloaded automatically after firing its last shot?")]
        [SerializeField]
        private bool automaticReloadOnEmpty;

        [Tooltip("Time after the last shot at which a reload will automatically start.")]
        [SerializeField]
        private float automaticReloadOnEmptyDelay = 0.25f;

        [SerializeField] private float maximumAimingDistance;
        [SerializeField] private float cooldownTime;
        [SerializeField] private LayerMask mask;

        [Title(label: "Animation")]

        [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
        [SerializeField]
        private Transform socketEjection;

        [Tooltip("Settings this to false will stop the weapon from being reloaded while the character is aiming it.")]
        [SerializeField]
        private bool canReloadAimed = true;

        [Title(label: "Resources")]

        [Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")]
        [SerializeField] 
        public RuntimeAnimatorController controller;

        [Tooltip("Weapon Body Texture.")]
        [SerializeField]
        private Sprite spriteBody;
        
        [Title(label: "Audio Clips Holster")]

        [Tooltip("Holster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipHolster;

        [Tooltip("Unholster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipUnholster;
        
        [Title(label: "Audio Clips Reloads")]

        [Tooltip("Reload Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReload;
        
        [Tooltip("Reload Empty Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadEmpty;
        
        [Title(label: "Audio Clips Reloads Cycled")]
        
        [Tooltip("Reload Open Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadOpen;
        
        [Tooltip("Reload Insert Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadInsert;
        
        [Tooltip("Reload Close Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadClose;
        
        [Title(label: "Audio Clips Other")]

        [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
        [SerializeField]
        private AudioClip audioClipFireEmpty;
        
        [Tooltip("")]
        [SerializeField]
        private AudioClip audioClipBoltAction;

        #endregion

        #region FIELDS

        /// <summary>
        /// Weapon Animator.
        /// </summary>
        private Animator animator;
        /// <summary>
        /// Attachment Manager.
        /// </summary>
        [SerializeField] private WeaponAttachmentManagerBehaviour attachmentManager;

        /// <summary>
        /// Amount of ammunition left.
        /// </summary>
        private int ammunitionCurrent;

        #region Attachment Behaviours
        
        /// <summary>
        /// Equiped scope Reference.
        /// </summary>
        private ScopeBehaviour scopeBehaviour;
        
        /// <summary>
        /// Equiped Magazine Reference.
        /// </summary>
        private MagazineBehaviour magazineBehaviour;
        /// <summary>
        /// Equiped Muzzle Reference.
        /// </summary>
        private MuzzleBehaviour muzzleBehaviour;

        #endregion

        /// <summary>
        /// The GameModeService used in this game!
        /// </summary>
        private IGameModeService gameModeService;
        /// <summary>
        /// The main player character behaviour component.
        /// </summary>
        private CharacterBehaviour characterBehaviour;

        /// <summary>
        /// The player character's camera.
        /// </summary>

        private Transform playerCameraTransform;

        public string Label => weaponName;
        public int RoundsPerMinutes => roundsPerMinutes;
        public int Damage => _damage * shotCount;
        public int HipSpread => (int)(100 - (spread * 100));
        public int AimSpread => (int)(100 - (scopeBehaviour.GetMultiplierSpread() * 100));
        public int Mobility => (int)(multiplierMovementSpeed * 100) - 20;
        public WeaponType Type => _type;
        public int Price => _price;
        public int Yan => _yan;
        public string Id => _id;
       
        public bool IsBought => _isBought;
        public bool IsEquip => _isEquip;
        public bool IsUnlock => _isUnlock;

        public event UnityAction Inited;
        public event UnityAction<Weapon> Bought;
        public event UnityAction<Weapon> Equiped;
        #endregion

        #region UNITY

        private void OnValidate()
        {
            _price = Mathf.Clamp(_price, 0, int.MaxValue);    
        }

        public void LoadStates(WeaponData data)
        {
            _isBought = data.IsBought;
            _isUnlock = data.IsUnlock;
            _isEquip = data.IsEquip;

            InitWeapon();
        }

        public void Buy()
        {
            _isBought = true;
            _isUnlock = true;
            Bought?.Invoke(this);
        }

        public void Equip()
        {
            _isEquip = true;
            Equiped?.Invoke(this);
        }

        public void Unlock()
        {
            _isUnlock = true;
        }

        public void UnEquip()
        {
            _isEquip = false;
            Equiped?.Invoke(this);
        }

        private void InitWeapon()
        {
            attachmentManager.InitWeaponAttachments();

            //Get Animator.
            animator = GetComponent<Animator>();
           
            //Cache the game mode service. We only need this right here, but we'll cache it in case we ever need it again.
            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            //Cache the player character.
            characterBehaviour = gameModeService.GetPlayerCharacter();

            scopeBehaviour = attachmentManager.GetEquippedScope();

            //Get Magazine.
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            //Get Muzzle.
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();

            #endregion
            //Max Out Ammo.
            ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();

            //Cache the world camera. We use this in line traces.

            SetCameraSettings();

            Inited?.Invoke();
        }

        private void SetCameraSettings()
        {
            //Cache the world camera. We use this in line traces.

            playerCameraTransform = characterBehaviour.GetCameraWorld().transform;
        }

        #region GETTERS

        /// <summary>
        /// GetFieldOfViewMultiplierAim.
        /// </summary>
        public override float GetFieldOfViewMultiplierAim()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null) 
                return scopeBehaviour.GetFieldOfViewMultiplierAim();
            
            //Error.
            Debug.LogError("Weapon has no scope equipped!");
  
            //Return.
            return 1.0f;
        }
        /// <summary>
        /// GetFieldOfViewMultiplierAimWeapon.
        /// </summary>
        public override float GetFieldOfViewMultiplierAimWeapon()
        {
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null) 
                return scopeBehaviour.GetFieldOfViewMultiplierAimWeapon();
            
            //Error.
            Debug.LogError("Weapon has no scope equipped!");
  
            //Return.
            return 1.0f;
        }
        
        /// <summary>
        /// GetAnimator.
        /// </summary>
        public override Animator GetAnimator() => animator;
        /// <summary>
        /// CanReloadAimed.
        /// </summary>
        public override bool CanReloadAimed() => canReloadAimed;

        /// <summary>
        /// GetSpriteBody.
        /// </summary>
        public override Sprite GetSpriteBody() => spriteBody;
        /// <summary>
        /// GetMultiplierMovementSpeed.
        /// </summary>
        public override float GetMultiplierMovementSpeed() => multiplierMovementSpeed;

        /// <summary>
        /// GetAudioClipHolster.
        /// </summary>
        public override AudioClip GetAudioClipHolster() => audioClipHolster;
        /// <summary>
        /// GetAudioClipUnholster.
        /// </summary>
        public override AudioClip GetAudioClipUnholster() => audioClipUnholster;

        /// <summary>
        /// GetAudioClipReload.
        /// </summary>
        public override AudioClip GetAudioClipReload() => audioClipReload;
        /// <summary>
        /// GetAudioClipReloadEmpty.
        /// </summary>
        public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;
        
        /// <summary>
        /// GetAudioClipReloadOpen.
        /// </summary>
        public override AudioClip GetAudioClipReloadOpen() => audioClipReloadOpen;
        /// <summary>
        /// GetAudioClipReloadInsert.
        /// </summary>
        public override AudioClip GetAudioClipReloadInsert() => audioClipReloadInsert;
        /// <summary>
        /// GetAudioClipReloadClose.
        /// </summary>
        public override AudioClip GetAudioClipReloadClose() => audioClipReloadClose;

        /// <summary>
        /// GetAudioClipFireEmpty.
        /// </summary>
        public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
        /// <summary>
        /// GetAudioClipBoltAction.
        /// </summary>
        public override AudioClip GetAudioClipBoltAction() => audioClipBoltAction;
        
        /// <summary>
        /// GetAudioClipFire.
        /// </summary>
        public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();
        /// <summary>
        /// GetAmmunitionCurrent.
        /// </summary>
        public override int GetAmmunitionCurrent() => ammunitionCurrent;

        /// <summary>
        /// GetAmmunitionTotal.
        /// </summary>
        public override int GetAmmunitionTotal() => magazineBehaviour.GetAmmunitionTotal();
        /// <summary>
        /// HasCycledReload.
        /// </summary>
        public override bool HasCycledReload() => cycledReload;

        /// <summary>
        /// IsAutomatic.
        /// </summary>
        public override bool IsAutomatic() => automatic;
        /// <summary>
        /// IsBoltAction.
        /// </summary>
        public override bool IsBoltAction() => boltAction;

        /// <summary>
        /// GetAutomaticallyReloadOnEmpty.
        /// </summary>
        public override bool GetAutomaticallyReloadOnEmpty(bool isMobile)
        {
           if(isMobile)
                automaticReloadOnEmpty = true;
           else
                automaticReloadOnEmpty = false;

            return automaticReloadOnEmpty;
        }

        /// <summary>
        /// GetAutomaticallyReloadOnEmptyDelay.
        /// </summary>
        public override float GetAutomaticallyReloadOnEmptyDelay() => automaticReloadOnEmptyDelay;

        /// <summary>
        /// CanReloadWhenFull.
        /// </summary>
        public override bool CanReloadWhenFull() => canReloadWhenFull;
        /// <summary>
        /// GetRateOfFire.
        /// </summary>
        public override float GetRateOfFire() => roundsPerMinutes;
        
        /// <summary>
        /// IsFull.
        /// </summary>
        public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();
        /// <summary>
        /// HasAmmunition.
        /// </summary>
        public override bool HasAmmunition() => ammunitionCurrent > 0;

        /// <summary>
        /// GetAnimatorController.
        /// </summary>
        public override RuntimeAnimatorController GetAnimatorController() => controller;
        /// <summary>
        /// GetAttachmentManager.
        /// </summary>
        public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachmentManager;

        #endregion

        #region METHODS

        /// <summary>
        /// Reload.
        /// </summary>
        public override void Reload()
        {
            //Set Reloading Bool. This helps cycled reloads know when they need to stop cycling.
            const string boolName = "Reloading";
            animator.SetBool(boolName, true);
            
            //Try Play Reload Sound.
            ServiceLocator.Current.Get<IAudioManagerService>().PlayOneShot(HasAmmunition() ? audioClipReload : audioClipReloadEmpty, new AudioSettings(_mixerGroup, 1.0f, 0.0f, false));
            
            //Play Reload Animation.
            animator.Play(cycledReload ? "Reload Open" : (HasAmmunition() ? "Reload" : "Reload Empty"), 0, 0.0f);
        }
        /// <summary>
        /// Fire.
        /// </summary>
        [SerializeField] private int _damage;
        public override void Fire(float timeAfterLastShot, float spreadMultiplier = 1.0f)
        {
            //We need a muzzle in order to fire this weapon!
            if (muzzleBehaviour == null)
                return;
            
            //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
            if (playerCameraTransform == null)
                return;

            //Play the firing animation.
            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);
            //Reduce ammunition! We just shot, so we need to get rid of one!
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

            //Set the slide back if we just ran out of ammunition.
            if (ammunitionCurrent == 0)
                SetSlideBack(1);
            
            //Play all muzzle effects.
            muzzleBehaviour.Effect();
            Transform muzzleSocket = muzzleBehaviour.GetSocket();
            //Spawn as many projectiles as we need.
            for (var i = 0; i < shotCount; i++)
            {
                Vector3 spreadValue = Vector3.zero;

                //Determine a random spread value using all of our multipliers.
                if (timeAfterLastShot < cooldownTime || shotCount > 1)
                {
                    spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                    //Remove the forward spread component, since locally this would go inside the object we're shooting!
                    spreadValue.z = 0;
                }

                //Convert to world space.
                spreadValue = playerCameraTransform.TransformDirection(spreadValue);

                //Spawn projectile from the projectile spawn point.
                Projectile projectile = _bulletPool.GetProjectile();
                projectile.SetDamage(_damage);

                Quaternion rotation = Quaternion.LookRotation(playerCameraTransform.forward * maximumAimingDistance + spreadValue - muzzleSocket.position);

                //If there's something blocking, then we can aim directly at that thing, which will result in more accurate shooting.
                if (Physics.Raycast(new Ray(playerCameraTransform.position, playerCameraTransform.forward), out RaycastHit hit, maximumAimingDistance, mask))
                    rotation = Quaternion.LookRotation(hit.point + spreadValue - muzzleSocket.position);

                _bulletPool.SetBulletTransform(projectile.gameObject, muzzleSocket.position, rotation);

                //Add velocity to the projectile.
                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
            }
        }

        /// <summary>
        /// FillAmmunition.
        /// </summary>
        public override void FillAmmunition(int amount)
        {
            //Update the value by a certain amount.
            ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount, 
                0, GetAmmunitionTotal()) : magazineBehaviour.GetAmmunitionTotal();
        }
        /// <summary>
        /// SetSlideBack.
        /// </summary>
        public override void SetSlideBack(int back)
        {
            //Set the slide back bool.
            const string boolName = "Slide Back";
            animator.SetBool(boolName, back != 0);
        }

        /// <summary>
        /// EjectCasing.
        /// </summary>
        public override void EjectCasing()
        {
            //Spawn casing prefab at spawn point.
            if (socketEjection != null)
            {
                GameObject casing = _casingPool.GetCasing();
                _casingPool.SetCasingTransform(casing, socketEjection.position, socketEjection.rotation);
            }
        }

        #endregion
    }
}