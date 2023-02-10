using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodShowerEffect;
    [SerializeField] private ParticleSystem _spawnSpinZoneEffect;
    [SerializeField] private ParticleSystem _dieSpinZonEffect;
    [SerializeField] private ParticleSystem _diePoolEffect;
    [SerializeField] private Vector3 _spawnSpanZoneOffset;
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
        if (_zombie.LastDamageHandlerType == DamageHandlerType.Head)
            PlayBloodShowerEffect();

        StartCoroutine(ShowDieEffects());
    }

    private void ShowSpawnEffect()
    {
        Instantiate(_spawnSpinZoneEffect.gameObject, transform.position + _spawnSpanZoneOffset, Quaternion.identity);
    }

    private IEnumerator ShowDieEffects()
    {
        yield return new WaitForSeconds(_delayBeforeDieEffect);

        //_diePoolEffect.Play();
        Instantiate(_diePoolEffect.gameObject, transform.position + _spawnSpanZoneOffset, _diePoolEffect.transform.rotation);
        Instantiate(_dieSpinZonEffect.gameObject, transform.position + _spawnSpanZoneOffset, Quaternion.identity);
    }

    private void PlayBloodShowerEffect()
    {
        _bloodShowerEffect.Play();
    }
}