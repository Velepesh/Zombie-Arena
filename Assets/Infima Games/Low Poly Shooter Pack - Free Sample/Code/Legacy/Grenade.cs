using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grenade : MonoBehaviour
	{
		[Header("Timer")]
		[Tooltip("Time before the grenade explodes")]
		[SerializeField] private float _grenadeTimer = 5.0f;
        [SerializeField] private float _groundDistance = 5.0f;
        [SerializeField] private int _damageToEnemy = 50;
        [SerializeField] private int _damageToPlayer = 19;
        [SerializeField] private int _damageToTwins = 9;

		[Header("Explosion Prefabs")]
        [SerializeField] private Explosion _explosion;

		[Header("Explosion Options")]
		[Tooltip("The radius of the explosion force")]
        [SerializeField] private float _radius = 25.0F;

		[Tooltip("The intensity of the explosion force")]
        [SerializeField] private float _power = 350.0F;

		[Header("Throw Force")]
		[Tooltip("Minimum throw force")]
        [SerializeField] private float _minimumForce = 1500.0f;

		[Tooltip("Maximum throw force")]
        [SerializeField] private float _maximumForce = 2500.0f;

		[Header("Audio")]
        [SerializeField] private AudioSource _impactSound;

		private float _throwForce;
		private Rigidbody _rigidbody;

        private void OnValidate()
        {
			_grenadeTimer = Mathf.Clamp(_grenadeTimer, 0f, float.MaxValue);
            _groundDistance = Mathf.Clamp(_groundDistance, 0f, float.MaxValue);
            _damageToEnemy = Mathf.Clamp(_damageToEnemy, 0, int.MaxValue);
            _damageToPlayer = Mathf.Clamp(_damageToPlayer, 0, int.MaxValue);
            _damageToTwins = Mathf.Clamp(_damageToTwins, 0, int.MaxValue);
            _radius = Mathf.Clamp(_radius, 0f, float.MaxValue);
            _power = Mathf.Clamp(_power, 0f, float.MaxValue);
            _minimumForce = Mathf.Clamp(_minimumForce, 0f, float.MaxValue);
            _maximumForce = Mathf.Clamp(_maximumForce, 0f, float.MaxValue);
        }

        private void Awake()
		{
			_throwForce = Random.Range(_minimumForce, _maximumForce);
			_rigidbody = GetComponent<Rigidbody>();

            _rigidbody.AddRelativeTorque(Random.Range(500, 1500), 0, 0 * Time.deltaTime * 5000);
		}

		private void Start()
		{
            _rigidbody.AddForce(gameObject.transform.forward * _throwForce);

			StartCoroutine(ExplosionTimer());
		}

		private void OnCollisionEnter(Collision collision)
		{
			_impactSound.Play();
		}

        private IEnumerator ExplosionTimer()
        {
            yield return new WaitForSeconds(_grenadeTimer);

            Explode();
            DoDamage();

            gameObject.SetActive(false);
        }

        private void InstantiateExplosion(Vector3 position, Quaternion rotation)
		{
            Instantiate(_explosion.gameObject, position, rotation);
        }

        private void Explode()
		{
            RaycastHit checkGround;
            if (Physics.Raycast(transform.position, Vector3.down, out checkGround, _groundDistance))
            {
                _explosion.EnableBurnMark();
                InstantiateExplosion(checkGround.point, Quaternion.FromToRotation(Vector3.forward, checkGround.normal));
            }
            else
            {
                _explosion.DisableBurnMark();
                InstantiateExplosion(transform.position, Quaternion.identity);
            }
        }

        private void DoDamage()
		{
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, _radius);

            bool itTwins = false;
            List<Zombie> zombies = new List<Zombie>();

            foreach (Collider hit in colliders)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    if (damageable is TwinCollider twin)
                    {
                        if(itTwins == false)
                            twin.TakeDamage(_damageToTwins, Vector3.zero);

                        itTwins = true;
                    }
                    else if(damageable is Player player)
                    {
                        player.TakeDamage(_damageToPlayer, Vector3.zero);
                    }
                    else if (damageable is DamageHandler damageHandler)
                    {
                        Zombie zombie = damageHandler.Zombie;
                        if (IsZombieInList(zombies, zombie) == false)
                        {
                            damageHandler.TakeDamage(_damageToEnemy, Vector3.zero);
                            zombies.Add(zombie);
                        }
                    }                       
                }
            }
        }

        private bool IsZombieInList(List<Zombie> zombies, Zombie zombie)
        {
            for (int i = 0; i < zombies.Count; i++)
            {
                if(zombie == zombies[i])
                    return true;
            }

            return false;
        }
	}
}