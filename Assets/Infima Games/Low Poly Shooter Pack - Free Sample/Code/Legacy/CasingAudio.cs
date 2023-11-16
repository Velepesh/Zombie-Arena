using Plugins.Audio.Utils;
using System;
using UnityEngine;
using System.Collections;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    public class CasingAudio : Audio
    {
        [SerializeField] private Casing _casing;
        [SerializeField] private AudioDataProperty[] _casingClips;
        [SerializeField] [Range(0, 1)] private float _minDelayTime;
        [SerializeField] [Range(0, 1)] private float _maxDelayTime;

        private void OnValidate()
        {
            if(_minDelayTime > _maxDelayTime)
                _minDelayTime = _maxDelayTime;
        }

        private void OnEnable()
        {
            _casing.Despawned += OnDespawned;
        }

        private void OnDisable()
        {
            _casing.Despawned -= OnDespawned;
        }

        private void OnDespawned()
        {
            StartCoroutine(PlaySound());
        }

        private IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minDelayTime, _maxDelayTime));
            AudioDataProperty property = _casingClips[UnityEngine.Random.Range(0, _casingClips.Length - 1)];

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            SourceAudio.PlayOneShot(property.Key);
        }
    }
}