using InfimaGames.LowPolyShooterPack.Legacy;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ImpactPool : ObjectPool
{
    [SerializeField] private Impact _impactPrefab;
    [SerializeField] private int _count;
    [SerializeField] private ImpactPoolType _type;

    private List<Impact> _impacts = new List<Impact>();

    public ImpactPoolType Type => _type;

    private void OnValidate()
    {
        _count = Mathf.Clamp(_count, 0, int.MaxValue);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _impacts.Count; i++)
            _impacts[i].Impacted -= OnImpacted;
    }

    private void Start()
    {
        StartGenerate();
    }

    public override void StartGenerate()
    {
        for (int i = 0; i < _count; i++)
            SpawnPrefab(_impactPrefab.gameObject);
    }

    public void SetBulletTransform(GameObject projectile, Vector3 position, Quaternion rotation)
    {
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;
    }

    public GameObject GetImpact()
    {
        if (TryGetObject(out GameObject bulletObject))
        {
            if (bulletObject.TryGetComponent(out Impact impact))
            {
                _impacts.Add(impact);
                impact.Impacted += OnImpacted;
            }
            else
            {
                throw new NullReferenceException(nameof(impact));
            }

            bulletObject.SetActive(true);

            return bulletObject;
        }

        throw new Exception("No active impact here");
    }

    private void OnImpacted(Impact impact)
    {
        _impacts.Remove(impact);
        impact.gameObject.SetActive(false);
    }
}

public enum ImpactPoolType
{
    Blood,
    Metal,
    Dirt,
    Concrete,
    Target,
    ExplosiveBarrel,
}