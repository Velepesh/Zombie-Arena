using Plugins.Audio.Utils;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    [RequireComponent(typeof(Grenade))]
    public class GrenadeAudio : Audio
    {
        [SerializeField] private AudioDataProperty _thudClip;

        private Grenade _grenade;

        private void Awake()
        {
            _grenade = GetComponent<Grenade>();
        }

        private void OnEnable()
        {
            _grenade.Collided += OnCollided;
        }

        private void OnDisable()
        {
            _grenade.Collided -= OnCollided;
        }

        private void OnCollided()
        {
            SourceAudio.PlayOneShot(_thudClip.Key);
        }
    }
}