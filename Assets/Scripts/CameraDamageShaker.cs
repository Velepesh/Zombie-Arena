using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDamageShaker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Range(0.001f, 0.01f)] [SerializeField] private float _shakeDecay = 0.002f;
    [Range(0.01f, 0.2f)] [SerializeField] private float _shakeIntensity = 0.02f;
    [Range(0.01f, 0.5f)] [SerializeField] private float _shakeAmount = 0.2f;
   
    private Coroutine _shakeJob;
    private Vector3 _originPosition;
    private Quaternion _originRotation;
    private float _intensity;

    private void OnEnable()
    {
        _player.Health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _player.Health.HealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        _originPosition = transform.localPosition;
        _originRotation = transform.localRotation;
    }

    private void OnHealthChanged(int health)
    {
        if (_shakeJob != null)
            StopCoroutine(_shakeJob);

        _shakeJob = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _intensity = _shakeIntensity;
        while (_intensity > 0)
        {
            transform.localPosition = _originPosition + Random.insideUnitSphere * _intensity;

            transform.localRotation = new Quaternion(
                _originRotation.x + Random.Range(-_intensity, _intensity) * _shakeAmount,
                _originRotation.y + Random.Range(-_intensity, _intensity) * _shakeAmount,
                _originRotation.z + Random.Range(-_intensity, _intensity) * _shakeAmount,
                _originRotation.w + Random.Range(-_intensity, _intensity) * _shakeAmount
                );

            _intensity -= _shakeDecay;
            yield return false;
        }

        transform.localPosition = _originPosition;
        transform.localRotation = _originRotation;
    }
}
