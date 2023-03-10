﻿using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// PlayOneShot Sound Behaviour. Plays an AudioClip using our custom AudioManager!
    /// </summary>
    public class PlaySoundBehaviour : StateMachineBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Header("Setup")]
        
        [Tooltip("AudioClip to play!")]
        [SerializeField]
        private AudioClip clip;
        
        [Header("Settings")]

        [Tooltip("Audio Settings.")]
        [SerializeField]
        private AudioSettings settings = new AudioSettings(null, 1.0f, 0.0f, true);

        /// <summary>
        /// Audio Manager Service. Handles all game audio.
        /// </summary>
        private IAudioManagerService audioManagerService;

        #endregion

        #region UNITY

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Try grab a reference to the sound managing service.
            audioManagerService ??= ServiceLocator.Current.Get<IAudioManagerService>();

            //PlayOneShot!
            audioManagerService?.PlayOneShot(clip, settings);
        }

        #endregion
    }
}