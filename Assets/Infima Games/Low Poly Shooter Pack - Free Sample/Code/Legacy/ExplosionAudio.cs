using Plugins.Audio.Utils;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    [RequireComponent(typeof(Explosion))]
    public class ExplosionAudio : Audio
    {
        [SerializeField] private AudioDataProperty _explosionClip;
        [SerializeField] private Explosion _explosion;

        private void OnEnable()
        {
            _explosion.Exploded += OnExploded;
        }

        private void OnDisable()
        {
            _explosion.Exploded -= OnExploded;
        }

        private void OnExploded()
        {
            SourceAudio.PlayOneShot(_explosionClip.Key);
        }
    }
}