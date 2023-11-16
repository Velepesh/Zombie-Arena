//Copyright 2022, Infima Games. All Rights Reserved.

using Plugins.Audio.Utils;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Play Sound Behaviour. Plays an AudioClip using our custom AudioManager!
    /// </summary>
    public class PlaySoundBehaviour : StateMachineBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "Setup")]
        
        [Tooltip("AudioClip to play!")]
        [SerializeField]
        private AudioDataProperty clip;
        
        [Title(label: "Settings")]

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

            //Play!
            audioManagerService?.PlayOneShot(clip, settings);
        }

        #endregion
    }
}