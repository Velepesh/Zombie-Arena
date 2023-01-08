// Copyright 2021, Infima Games. All Rights Reserved.

using System.Collections;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon. This class handles most of the things that weapons need.
    /// </summary>
    public class Weapon : WeaponBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Header("Firing")]

        [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
        [SerializeField] 
        private bool automatic;

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

        [Tooltip("Mask of things recognized when firing.")]
        [SerializeField]
        private LayerMask mask;

        [Tooltip("Maximum distance at which this weapon can fire accurately. Shots beyond this distance will not use linetracing for accuracy.")]
        [SerializeField]
        private float maximumDistance = 500.0f;

        [Header("Animation")]

        [Tooltip("Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
        [SerializeField]
        private Transform socketEjection;

        [Header("Resources")]

        [Tooltip("Casing Prefab.")]
        [SerializeField]
        private GameObject prefabCasing;
        
        [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")]
        [SerializeField]
        private GameObject prefabProjectile;
        
        [Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")]
        [SerializeField] 
        public RuntimeAnimatorController controller;

        [Tooltip("Weapon Body Texture.")]
        [SerializeField]
        private Sprite spriteBody;
        
        [Header("Audio Clips Holster")]

        [Tooltip("Holster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipHolster;

        [Tooltip("Unholster Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipUnholster;
        
        [Header("Audio Clips Reloads")]

        [Tooltip("Reload Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReload;
        
        [Tooltip("Reload Empty Audio Clip.")]
        [SerializeField]
        private AudioClip audioClipReloadEmpty;
        
        [Header("Audio Clips Other")]

        [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
        [SerializeField]
        private AudioClip audioClipFireEmpty;

        [SerializeField] private Vector2[] _recoilPattern;
        [SerializeField] private float _duration;
        [SerializeField] private float _recoilDivision;

        private int _index;
        private float _time;
        private float _recoilAmountY;
        private float _recoilAmountX;
        private float _recoilX;
        private float _recoilY;

        #endregion

        #region FIELDS

        /// <summary>
        /// Weapon Animator.
        /// </summary>
        private Animator animator;
        /// <summary>
        /// Attachment Manager.
        /// </summary>
        private WeaponAttachmentManagerBehaviour attachmentManager;

        /// <summary>
        /// Amount of ammunition left.
        /// </summary>
        private int ammunitionCurrent;

        #region Attachment Behaviours
        
        /// <summary>
        /// Equipped Magazine Reference.
        /// </summary>
        private MagazineBehaviour magazineBehaviour;
        /// <summary>
        /// Equipped Muzzle Reference.
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
        private float _aimFieldOfView ;
        private float _scopingCameraSpeed;

        private Camera _playerCamera;
        private float _startCameraFieldOfView;
        private Vector3 _startCameraPosition;
        private Vector3 _aimCameraPosition;

        #endregion

        #region UNITY

        protected override void Awake()
        {
            //Get Animator.
            animator = GetComponent<Animator>();
            //Get Attachment Manager.
            attachmentManager = GetComponent<WeaponAttachmentManagerBehaviour>();

            //Cache the game mode service. We only need this right here, but we'll cache it in case we ever need it again.
            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            //Cache the player character.
            characterBehaviour = gameModeService.GetPlayerCharacter();
            //Cache the world camera. We use this in line traces.
            SetCameraSettings();
        }

        private void SetCameraSettings()
        {
            //Cache the world camera. We use this in line traces.
            _playerCamera = characterBehaviour.GetCameraWorld();
            playerCameraTransform = _playerCamera.transform;
            _startCameraFieldOfView = _playerCamera.fieldOfView;

            _startCameraPosition = playerCameraTransform.localPosition;
            _aimFieldOfView = characterBehaviour.GetAimCameraFieldOfView();
            _scopingCameraSpeed = characterBehaviour.GetScopingCameraSpeed();
            _aimCameraPosition = characterBehaviour.GetAimCameraPosition();
        }

        protected override void Start()
        {
            #region Cache Attachment References
            
            //Get Magazine.
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            //Get Muzzle.
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();

            #endregion

            //Max Out Ammo.
            ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();
        }

        #endregion

        #region GETTERS

        public override Animator GetAnimator() => animator;
        
        public override Sprite GetSpriteBody() => spriteBody;

        public override AudioClip GetAudioClipHolster() => audioClipHolster;
        public override AudioClip GetAudioClipUnholster() => audioClipUnholster;

        public override AudioClip GetAudioClipReload() => audioClipReload;
        public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;

        public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
        
        public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();
        
        public override int GetAmmunitionCurrent() => ammunitionCurrent;

        public override int GetAmmunitionTotal() => magazineBehaviour.GetAmmunitionTotal();

        public override bool IsAutomatic() => automatic;
        public override float GetRateOfFire() => roundsPerMinutes;
        
        public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();
        public override bool HasAmmunition() => ammunitionCurrent > 0;

        public override RuntimeAnimatorController GetAnimatorController() => controller;
        public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachmentManager;

        #endregion

        #region METHODS

        public override void Reload()
        {
            //Play Reload Animation.
            animator.Play(HasAmmunition() ? "Reload" : "Reload Empty", 0, 0.0f);
        }

        public override void Recoil()
        {
            if (_time > 0)
            {
                _recoilX += _recoilAmountX / _recoilDivision * Time.deltaTime / _duration;
                _recoilY = _recoilAmountY / _recoilDivision * Time.deltaTime / _duration;

                _time -= Time.deltaTime;
                return;
            }

            if (_recoilY != 0)
            {
                _recoilX = 0;
                _recoilY = 0;
            }
        }

        public override void GenerateRecoil()
        {
            _time = _duration;

            _recoilAmountX = _recoilPattern[_index].x;
            _recoilAmountY = _recoilPattern[_index].y;

            _index = NextIndex(_index);
        }

        public override float GetRecoilY()
        {
            return _recoilY;
        }

        private int NextIndex(int index)
        {
            return (index + 1) % _recoilPattern.Length;
        }

        public override void TryResetIndex()
        {
            if (_time <= 0)
                _index = 0;
        }

        public override void Fire(float spreadMultiplier = 1.0f)
        {
            //We need a muzzle in order to fire this weapon!
            if (muzzleBehaviour == null)
                return;

            //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
            if (_playerCamera == null)
                return;

            //Get Muzzle Socket. This is the point we fire from.
            Transform muzzleSocket = muzzleBehaviour.GetSocket();

            //Play the firing animation.
            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);
            //Reduce ammunition! We just shot, so we need to get rid of one!
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

            //Play all muzzle effects.
            muzzleBehaviour.Effect();
            GenerateRecoil();
            //Spawn as many projectiles as we need.
            for (var i = 0; i < shotCount; i++)
            {
                //Determine a random spread value using all of our multipliers.
                Vector3 spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                //Remove the forward spread component, since locally this would go inside the object we're shooting!
                spreadValue.z = 0;
                //Convert to world space.
                spreadValue = muzzleSocket.TransformDirection(spreadValue) + new Vector3(_recoilX, 0f, 0f);

                //Determine the rotation that we want to shoot our projectile in.
                Quaternion rotation = Quaternion.LookRotation(playerCameraTransform.forward * 1000.0f + spreadValue - muzzleSocket.position);

                //If there's something blocking, then we can aim directly at that thing, which will result in more accurate shooting.
                if (Physics.Raycast(new Ray(playerCameraTransform.position, playerCameraTransform.forward),
                    out RaycastHit hit, maximumDistance, mask))
                    rotation = Quaternion.LookRotation(hit.point + spreadValue - muzzleSocket.position);

                //Spawn projectile from the projectile spawn point.
                GameObject projectile = Instantiate(prefabProjectile, muzzleSocket.position, rotation);

                //Add velocity to the projectile.
                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
            }
        }

        public override void Scope()
        {
            OnScope(_aimFieldOfView, _aimCameraPosition);
        }

        public override void Unscope()
        {
            OnScope(_startCameraFieldOfView, _startCameraPosition);
        }

        public override void FillAmmunition(int amount)
        {
            //Update the value by a certain amount.
            ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount, 
                0, GetAmmunitionTotal()) : magazineBehaviour.GetAmmunitionTotal();
        }

        public override void EjectCasing()
        {
            //Spawn casing prefab at spawn point.
            if(prefabCasing != null && socketEjection != null)
                Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
        }

        #endregion

        private void OnScope(float targetFieldOfView, Vector3 targetCameraPosition)
        {
            _playerCamera.fieldOfView = Mathf.MoveTowards(_playerCamera.fieldOfView, targetFieldOfView, _scopingCameraSpeed * Time.deltaTime);
            playerCameraTransform.localPosition = Vector3.MoveTowards(playerCameraTransform.localPosition, targetCameraPosition, _scopingCameraSpeed * Time.deltaTime);
        }
    }
}