using UnityEngine;
using System.Collections;
using UnityEngine.Events;

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

        public event UnityAction Exploded;


        private void Start() 
        {
    		StartCoroutine(DestroyTimer());
    		StartCoroutine(LightFlash());
    
            Exploded?.Invoke();
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