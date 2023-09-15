using InfimaGames.LowPolyShooterPack.Legacy;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : ObjectPool
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private List<ImpactPool> _impactPools;
    [SerializeField] private int _numberOfBullets;

    private List<Projectile> _projectiles = new List<Projectile>();

    public int Damage =>_projectile.Damage;

    private void OnValidate()
    {
        _numberOfBullets = Mathf.Clamp(_numberOfBullets, 0, int.MaxValue);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _projectiles.Count; i++)
            _projectiles[i].Impacted -= OnImpacted;
    }

    public override void GeneratePrefabs()
    {
        for (int i = 0; i < _numberOfBullets; i++)
        {
            Projectile projectile = SpawnPrefab(_projectile.gameObject).GetComponent<Projectile>();
            projectile.SetImpactPools(_impactPools);
        }
    }

    public void SetBulletTransform(GameObject projectile, Vector3 position, Quaternion rotation)
    {
        projectile.transform.position = position;
        projectile.transform.rotation = rotation;
    }

    public GameObject GetBullet()
    {
        if(TryGetObject(out GameObject bulletObject))
        {
            if (bulletObject.TryGetComponent(out Projectile projectile))
            {
                _projectiles.Add(projectile);
                projectile.Impacted += OnImpacted;
            }
            else
            {
                throw new NullReferenceException(nameof(projectile));
            }
            
            bulletObject.SetActive(true);

            return bulletObject;
        }

        throw new Exception("No active projectile here");
    }

    private void OnImpacted(Projectile projectile)
    {
        _projectiles.Remove(projectile);
        projectile.gameObject.SetActive(false);
    }
}