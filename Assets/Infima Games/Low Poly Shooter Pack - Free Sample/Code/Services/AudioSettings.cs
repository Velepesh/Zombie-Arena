//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.Audio;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Audio VolumeView used to interact with the AudioManagerService.
    /// </summary>
    [System.Serializable]
    public struct AudioSettings
    {
        /// <summary>
        /// Automatic Cleanup Getter.
        /// </summary>
        public bool AutomaticCleanup => automaticCleanup;
        /// <summary>
        /// Volume Getter.
        /// </summary>
        public float Volume => volume;
        /// <summary>
        /// Spatial Blend Getter.
        /// </summary>
        public float SpatialBlend => spatialBlend;
        public AudioMixerGroup MixerGroup => mixerGroup;

        [Header("Settings")]
        
        [Tooltip("If true, any AudioSource created will be removed after it has finished playing its clip.")]
        [SerializeField]
        private bool automaticCleanup;

        [Tooltip("Volume.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float volume;

        [Tooltip("Spatial Blend.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float spatialBlend;

        [Tooltip("AudioMixer.")]
        [SerializeField]
        private AudioMixerGroup mixerGroup;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioSettings(AudioMixerGroup mixerGroup, float volume = 1.0f, float spatialBlend = 0.0f, bool automaticCleanup = true)
        {
            this.mixerGroup = mixerGroup;
            //Volume.
            this.volume = volume;
            //Spatial Blend.
            this.spatialBlend = spatialBlend;
            //Automatic Cleanup.
            this.automaticCleanup = automaticCleanup;
        }
    }
}