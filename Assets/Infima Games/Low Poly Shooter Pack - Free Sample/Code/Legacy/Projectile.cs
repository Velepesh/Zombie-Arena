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
        //[SerializeField] private int _damage;
        private int _damage;

        [Range(5, 100)]
		[Tooltip("After how long time should the bullet prefab be destroyed?")]
		public float destroyAfter;

		[Tooltip("If enabled the bullet destroys on impact")]
		public bool destroyOnImpact = false;

		[Tooltip("Minimum time after impact that the bullet is destroyed")]
		public float minDestroyTime;

		[Tooltip("Maximum time after impact that the bullet is destroyed")]
		public float maxDestroyTime;

        [SerializeField] private float _impactOffsetOnDamageHandler;
        [SerializeField] private ParticleSystem _bulletHoleEffect;
        [SerializeField] private ParticleSystem _metalHoleEffect;
		
		private List<ImpactPool> _impactPools = new List<ImpactPool>();
        private int _damageOnTwins => Mathf.CeilToInt((float)(_damage * 0.1f));

        public int Damage => _damage;
		public event UnityAction<Projectile> Impacted;

        private void OnEnable()
        {
            StartCoroutine(DestroyAfter());
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
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

            //If destroy on impact is false, start coroutine with random destroy timer
            if (!destroyOnImpact)
				StartCoroutine(DestroyTimer());
            else//Otherwise, destroy bullet on impact
                DisableProjectile();

            var contact = collision.contacts[0];

            Vector3 normal = contact.normal;
            Vector3 point = contact.point;

            if (collision.gameObject.TryGetComponent(out DamageHandler damageHandler))
            {
                InstantiateBulletHole(damageHandler, collision.gameObject.transform, point);
                PlayImpact(ImpactPoolType.Blood, point - transform.TransformDirection(new Vector3(0, 0, _impactOffsetOnDamageHandler)), normal);
                damageHandler.TakeDamage(_damage, normal);
            }

            if (collision.gameObject.TryGetComponent(out TwinCollider twinCollider))
            {
                InstantiateHole(_metalHoleEffect, collision.gameObject.transform, point);
                PlayImpact(ImpactPoolType.Metal, point, normal);
                twinCollider.TakeDamage(_damageOnTwins, normal);
            }

            if (collision.transform.tag == "Grass")
                PlayImpact(ImpactPoolType.Grass, point, normal);
           
            if (collision.transform.tag == "Concrete")
				PlayImpact(ImpactPoolType.Concrete, point, normal);


            if (collision.transform.tag == "ForceField")
                PlayImpact(ImpactPoolType.ForceField, point, normal);
        }

        private void DisableProjectile()
        {
            Impacted?.Invoke(this);
        }

        private void InstantiateBulletHole(DamageHandler damageHandler, Transform collisionTranform, Vector3 point)
        {
            GameObject hole = InstantiateHole(_bulletHoleEffect, collisionTranform, point);

            if(damageHandler.Type == DamageHandlerType.Head)
                damageHandler.AddHoleEffect(hole.GetComponent<ParticleSystem>());
        }  

        private GameObject InstantiateHole(ParticleSystem holeEffect, Transform collisionTranform, Vector3 point)
        {
            GameObject hole = Instantiate(holeEffect.gameObject, collisionTranform);
            hole.transform.position = point;

            return hole;
        }

        private void PlayImpact(ImpactPoolType type, Vector3 point, Vector3 contactNormal)
        {
            for (int i = 0; i < _impactPools.Count; i++)
            {
                if (_impactPools[i].Type == type)
                {
                    GameObject impact = _impactPools[i].GetImpact();
                    impact.SetActive(true);
                    _impactPools[i].SetImpactTransform(impact.transform, point, Quaternion.LookRotation(contactNormal));
                    break;
                }
            }

            DisableProjectile();
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