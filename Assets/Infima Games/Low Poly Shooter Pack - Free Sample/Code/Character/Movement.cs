// Copyright 2021, Infima Games. All Rights Reserved.

using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InfimaGames.LowPolyShooterPack
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Movement : MovementBehaviour
    {
        /// <summary>
        /// A helper for assistance with smoothing the movement.
        /// </summary>
        private class SmoothVelocity
        {
            /// <summary>
            /// Value Getter.
            /// </summary>
            public Vector3 Value { get; private set; }

            /// <summary>
            /// Current Velocity. Used for proper SmoothDamp use.
            /// </summary>
            private Vector3 currentVelocity;

            /// <summary>
            /// Updates the value!
            /// </summary>
            /// <param name="target">Target value.</param>
            /// <param name="smoothTime">How smooth the motion should be.</param>
            /// <returns></returns>
            public Vector3 Update(Vector3 target, float smoothTime)
            {
                float velocityY = target.y;
                target = new Vector3(target.x, 0f, target.z);
                Value = Vector3.SmoothDamp(Value, target, ref currentVelocity, smoothTime);

                return Value + new Vector3(0f, velocityY, 0f);
            }
        }

        #region FIELDS SERIALIZED

        [Header("Audio Clips")]
        
        [Tooltip("The audio clip that is played while walking.")]
        [SerializeField]
        private AudioClip audioClipWalking;

        [Tooltip("The audio clip that is played while running.")]
        [SerializeField]
        private AudioClip audioClipRunning;

        [Header("Speeds")]

        [SerializeField]
        private float speedWalking = 5.0f;

        [Tooltip("How fast the player moves while running."), SerializeField]
        private float speedRunning = 9.0f;

        [Tooltip("Jump height"), SerializeField]
        private float jumpHeight = 3;

        [Header("Interpolation")]

        [Tooltip("Approximately the amount of time it will take for the player to reach maximum running or walking speed.")]
        [SerializeField]
        private float movementSmoothness = 0.125f;
        #endregion

        #region PROPERTIES

        //Velocity.
        private Vector3 Velocity
        {
            //Getter.
            get => rigidBody.velocity;
            //Setter.
            set => rigidBody.velocity = smoothVelocity.Update(value, movementSmoothness);
        }

        #endregion

        #region FIELDS

        /// <summary>
        /// Attached Rigidbody.
        /// </summary>
        private Rigidbody rigidBody;
        /// <summary>
        /// Attached CapsuleCollider.
        /// </summary>
        private CapsuleCollider capsule;
        /// <summary>
        /// Attached AudioSource.
        /// </summary>
        private AudioSource audioSource;
        /// <summary>
        /// Velocity Smoothing Helper. Basically does what it says, it helps us make the velocity smoother.
        /// </summary>
        private SmoothVelocity smoothVelocity;

        /// <summary>
        /// True if the character is currently grounded.
        /// </summary>
        private bool grounded;

        /// <summary>
        /// Player Character.
        /// </summary>
        private CharacterBehaviour playerCharacter;
        /// <summary>
        /// The player character's equipped weapon.
        /// </summary>
        private WeaponBehaviour equippedWeapon;
        
        /// <summary>
        /// Array of RaycastHits used for ground checking.
        /// </summary>
        private readonly RaycastHit[] groundHits = new RaycastHit[8];

        private float velocityY;
        [SerializeField] private float gravity = 9.81f;

        private void OnValidate()
        {
            gravity = Mathf.Clamp(gravity, 0f, float.MaxValue);
        }
        #endregion

        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        protected override void Awake()
        {
            //Get Player Character.
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        }

        /// Initializes the FpsController on start.
        protected override void Start()
        {
            //Rigidbody Setup.
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            //Cache the CapsuleCollider.
            capsule = GetComponent<CapsuleCollider>();

            //Audio Source Setup.
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClipWalking;
            audioSource.loop = true;

            //Create our smooth velocity helper. Will be useful to get some smoother motion.
            smoothVelocity = new SmoothVelocity();
        }

        /// Checks if the character is on the ground.
        private void OnCollisionStay()
        {
            //Bounds.
            Bounds bounds = capsule.bounds;
            //Extents.
            Vector3 extents = bounds.extents;
            //Radius.
            float radius = extents.x - 0.01f;
            
            //Cast. This checks whether there is indeed ground, or not.
            Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
                groundHits, extents.y - radius * 0.65f, ~0, QueryTriggerInteraction.Ignore);
            
            //We can ignore the rest if we don't have any proper hits.
            if (!groundHits.Any(hit => hit.collider != null && hit.collider != capsule)) 
                return;
            
            //Store RaycastHits.
            for (var i = 0; i < groundHits.Length; i++)
                groundHits[i] = new RaycastHit();

            //Set grounded. Now we know for sure that we're grounded.
            grounded = true;
        }
			
        protected override void FixedUpdate()
        {
            //MoveDown.
            MoveCharacter();
            
            //Unground.
            grounded = false;
        }

        /// Moves the camera to the character, processes jumping and plays sounds every frame.
        protected override  void Update()
        {
            //Get the equipped weapon!
            equippedWeapon = playerCharacter.GetInventory().GetEquipped();
            
            //Play Sounds!
            PlayFootstepSounds();
        }

        #endregion

        #region METHODS

        /// <summary>
		/// Impact.
		/// </summary>
		public void OnJump(InputAction.CallbackContext context)
        {
            if (grounded)
                velocityY = Mathf.Sqrt(jumpHeight * -2f * (-gravity));
        }

        private void MoveCharacter()
        {
            #region Calculate Movement Velocity

            //Get Movement Input!
            Vector2 frameInput = playerCharacter.GetInputMovement();
            //Calculate local-space direction by using the player's input.
            var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);
            
            //Running speed calculation.
            if(playerCharacter.IsRunning())
                movement *= speedRunning;
            else//Multiply by the normal walking speed.
                movement *= speedWalking;

            //World space velocity calculation. This allows us to add it to the rigidbody's velocity properly.
            movement = transform.TransformDirection(movement);
            if(grounded == false)
            {
                velocityY -= gravity * Time.fixedDeltaTime;
            }
            else
            {
                if(velocityY <= 0f)
                    velocityY = 0;
            }

            #endregion
            //Update Velocity.
            
            Velocity = new Vector3(movement.x, velocityY, movement.z);
        }
        float currentJumpStartTime;
        Vector3 momentum;
        void OnJumpStart()
        {
            //If local momentum is used, transform momentum into world coordinates first;
                momentum = transform.localToWorldMatrix * momentum;

            //Add jump force to momentum;
            momentum += transform.up * jumpHeight;

            //Set jump start time;
            currentJumpStartTime = Time.time;

            //Lock jump input until jump key is released again;

          

                momentum = transform.worldToLocalMatrix * momentum;
        }

        /// <summary>
        /// Plays Footstep Sounds. This code is slightly old, so may not be great, but it functions alright-_recoilY!
        /// </summary>
        private void PlayFootstepSounds()
        {
            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (grounded && rigidBody.velocity.sqrMagnitude > 0.1f)
            {
                //Select the correct audio clip to play.
                audioSource.clip = playerCharacter.IsRunning() ? audioClipRunning : audioClipWalking;
                //Play it!
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            //Pause it if we're doing something like flying, or not moving!
            else if (audioSource.isPlaying)
                audioSource.Pause();
        }

        #endregion
    }
}