using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.Events;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	public class Projectile : MonoBehaviour
	{
        [SerializeField] private int _damage;

        [Range(5, 100)]
		[Tooltip("After how long time should the bullet prefab be destroyed?")]
		public float destroyAfter;

		[Tooltip("If enabled the bullet destroys on impact")]
		public bool destroyOnImpact = false;

		[Tooltip("Minimum time after impact that the bullet is destroyed")]
		public float minDestroyTime;

		[Tooltip("Maximum time after impact that the bullet is destroyed")]
		public float maxDestroyTime;

		[Header("Impact Effect Prefabs")]
		public Transform[] bloodImpactPrefabs;
		public Transform[] metalImpactPrefabs;
		public Transform[] dirtImpactPrefabs;
		public Transform[] concreteImpactPrefabs;
		
		private List<ImpactPool> _impactPools = new List<ImpactPool>();

		public int Damage => _damage;
		public event UnityAction<Projectile> Impacted;

        private void OnEnable()
        {
            StartCoroutine(DestroyAfter());
        }

		public void SetImpactPools(List<ImpactPool> impactPools)
		{
			_impactPools = impactPools;
        }

		//If the bullet collides with anything
		private void OnCollisionEnter(Collision collision)
		{
			if (_impactPools.Count == 0)
				throw new ArgumentNullException(nameof(_impactPools));
			//Ignore collisions with other projectiles.
			if (collision.gameObject.GetComponent<Projectile>() != null)
				return;

            //If destroy on impact is false, start 
            //coroutine with random destroy timer
            if (!destroyOnImpact)
			{
				StartCoroutine(DestroyTimer());
			}
			//Otherwise, destroy bullet on impact
			else
			{
                DisableProjectile();
            }

            if (collision.gameObject.TryGetComponent(out DamageHandler damageHandler))
            {
                damageHandler.TakeDamage(_damage, collision.contacts[0].normal);
                PlayImpact(ImpactPoolType.Blood, collision.contacts[0].normal);

                DisableProjectile();
            }

			if (collision.transform.tag == "Concrete")
			{
				PlayImpact(ImpactPoolType.Concrete, collision.contacts[0].normal);
                DisableProjectile();
            }
		}

        private void DisableProjectile()
        {
            Impacted?.Invoke(this);
        }

        private void PlayImpact(ImpactPoolType type, Vector3 contactNormal)
        {
            for (int i = 0; i < _impactPools.Count; i++)
            {
                if (_impactPools[i].Type == type)
                {
                    GameObject impact = _impactPools[i].GetImpact();
                    _impactPools[i].SetImpactTransform(impact, transform.position, Quaternion.LookRotation(contactNormal));
                    break;
                }
            }
        }

        private IEnumerator DestroyTimer()
		{
			//Wait random time based on min and max values
			yield return new WaitForSeconds
				(Random.Range(minDestroyTime, maxDestroyTime));
            
			DisableProjectile();
        }

		private IEnumerator DestroyAfter()
		{
			//Wait for set amount of time
			yield return new WaitForSeconds(destroyAfter);

            DisableProjectile();
        }
	}
}