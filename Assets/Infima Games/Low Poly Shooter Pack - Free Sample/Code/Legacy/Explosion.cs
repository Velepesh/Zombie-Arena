using UnityEngine;
using System.Collections;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	public class Explosion : MonoBehaviour 
    {
    	[Header("Customizable Options")]
        [SerializeField] private float _despawnTime = 4f;
        [SerializeField] private float _lightDuration = 0.05f;

    	[Header("Light")]
        [SerializeField] private Light _lightFlash;
        [SerializeField] private ParticleSystem _burnMark;
    
    	[Header("Audio")]
        [SerializeField] private AudioClip[] _explosionSounds;
        [SerializeField] private AudioSource _audioSource;
    
    	private void Start () 
        {
    		StartCoroutine(DestroyTimer());
    		StartCoroutine(LightFlash());
    
    		_audioSource.clip = _explosionSounds[Random.Range(0, _explosionSounds.Length)];
    		_audioSource.Play();
    	}

        public void EnableBurnMark()
        {
            _burnMark.gameObject.SetActive(true);
        }

        public void DisableBurnMark()
        {
            _burnMark.gameObject.SetActive(false);
        }

        private IEnumerator LightFlash () 
        {
    		_lightFlash.enabled = true;

    		yield return new WaitForSeconds (_lightDuration);
    		_lightFlash.enabled = false;
    	}
    
    	private IEnumerator DestroyTimer () 
        {
    		yield return new WaitForSeconds (_despawnTime);
    		gameObject.SetActive(false);
    	}
    }
}