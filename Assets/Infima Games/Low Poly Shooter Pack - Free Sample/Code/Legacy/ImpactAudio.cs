using Plugins.Audio.Utils;
using UnityEngine;
using System;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    public class ImpactAudio : Audio
    {
        [SerializeField] private Impact _impact;
        [SerializeField] private AudioDataProperty[] _impactClips;

        private void OnEnable()
        {
            _impact.Despawned += OnDespawned;
        }

        private void OnDisable()
        {
            _impact.Despawned -= OnDespawned;
        }

        private void OnDespawned()
        {
            AudioDataProperty property = _impactClips[UnityEngine.Random.Range(0, _impactClips.Length - 1)];
            
            if(property == null)
                throw new ArgumentNullException(nameof(property));

            SourceAudio.PlayOneShot(property.Key);
        }
    }
}