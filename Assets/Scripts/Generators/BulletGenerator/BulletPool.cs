using InfimaGames.LowPolyShooterPack.Legacy;
using System;
using UnityEngine;

public class BulletPool : ObjectPool
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private int _numberOfBullets;

    private void OnValidate()
    {
        _numberOfBullets = Mathf.Clamp(_numberOfBullets, 0, int.MaxValue);
    }

    private void Start()
    {
        StartGenerate();
    }

    public override void StartGenerate()
    {
        for (int i = 0; i < _numberOfBullets; i++)
            SpawnPrefab(_projectile.gameObject);
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
            bulletObject.SetActive(true);

            return bulletObject;
        }

        throw new Exception("No active projectile here");
    }
}
