//Copyright 2022, Infima Games. All Rights Reserved.

using Plugins.Audio.Core;
using Plugins.Audio.Utils;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Footstep Player. This component is in charge of playing footstep sounds for the player.
    /// We have this a separate component so as to make sure that it can be super easily removed, and replaced
    /// for a more custom implementation, as our setup is quite basic at the moment.
    /// </summary>
    public class FootstepPlayer : MonoBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "References")]

        [Tooltip("The character's Movement Behaviour component.")]
        [SerializeField, NotNull]
        private MovementBehaviour movementBehaviour;

        [Tooltip("The character's Animator component.")]
        [SerializeField, NotNull]
        private Animator characterAnimator;
        
        [Tooltip("The character's footstep-dedicated Audio Source component.")]
        [SerializeField, NotNull]
        private SourceAudio sourceAudio;

        [Title(label: "Settings")]

        [Tooltip("Minimum magnitude of the movement velocity at which the audio clips will start playing.")]
        [SerializeField]
        private float minVelocityMagnitude = 1.0f;
        
        [Title(label: "Audio Clips")]
        
        [Tooltip("The audio clip that is played while walking.")]
        [SerializeField] private AudioDataProperty audioClipWalking;

        [Tooltip("The audio clip that is played while running.")]
        [SerializeField] private AudioDataProperty audioClipRunning;

        private bool _isPlaying;
        #endregion

        #region UNITY

        /// <summary>
        /// Update.
        /// </summary>
        private void Update()
        {
            //Check for missing references.
            if (characterAnimator == null || movementBehaviour == null || sourceAudio == null)
            {
                //Reference Error.
                Log.ReferenceError(this, gameObject);
                
                //Return.
                return;
            }

            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (movementBehaviour.IsGrounded() && movementBehaviour.GetVelocity().sqrMagnitude > minVelocityMagnitude)
            {
                //Select the correct audio clip to play.
                AudioDataProperty clip = characterAnimator.GetBool(AHashes.Running) ? audioClipRunning : audioClipWalking;
                //Play it!
                if (_isPlaying == false)
                {
                    sourceAudio.Play(clip.Key);
                    _isPlaying = true;
                }
            }
            //Pause it if we're doing something like flying, or not moving!
            else if (_isPlaying)
            {
                sourceAudio.Stop();
                _isPlaying = false;
            }
        }
        
        #endregion
    }
}