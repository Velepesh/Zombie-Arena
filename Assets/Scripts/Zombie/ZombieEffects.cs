using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _spawnEffect;
    [SerializeField] private ParticleSystem _dieEffect;
    [SerializeField] private Vector3 _spawnOffset;
    [SerializeField] private float _delayBeforeDieEffect;

    private Zombie _zombie;

    private void OnValidate()
    {
        _delayBeforeDieEffect = Mathf.Clamp(_delayBeforeDieEffect, 0f, float.MaxValue);
    }

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        _zombie.Died += OnDied;
    }

    private void OnDisable()
    {
        _zombie.Died -= OnDied;
    }

    private void Start()
    {
        ShowSpawnEffect();
    }

    private void OnDied(IDamageable damageable)
    {
        StartCoroutine(ShowDieEffect());
    }

    private void ShowSpawnEffect()
    {
        Instantiate(_spawnEffect.gameObject, transform.position + _spawnOffset, Quaternion.identity);
    }

    private IEnumerator ShowDieEffect()
    {
        yield return new WaitForSeconds(_delayBeforeDieEffect);

        Instantiate(_dieEffect.gameObject, transform.position + _spawnOffset, Quaternion.identity);
    }
}