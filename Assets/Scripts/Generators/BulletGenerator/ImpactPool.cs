using InfimaGames.LowPolyShooterPack.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactPool : MonoBehaviour
{
    //public Impact _bloodImpactPrefab;
    //public Impact _metalImpactPrefab;
    //public Impact _dirtImpactPrefab;
    //public Impact _concreteImpactPrefab;

    //[SerializeField] private int _count;

    //private List<Projectile> _projectiles = new List<Projectile>();

    //private void OnValidate()
    //{
    //    _count = Mathf.Clamp(_count, 0, int.MaxValue);
    //}

    //private void OnDisable()
    //{
    //    for (int i = 0; i < _projectiles.Count; i++)
    //        _projectiles[i].Impacted -= OnImpacted;
    //}

    //private void Start()
    //{
    //    StartGenerate();
    //}

    //public override void StartGenerate()
    //{
    //    for (int i = 0; i < _count; i++)
    //        SpawnPrefab(_projectile.gameObject);
    //}

    //public void SetBulletTransform(GameObject projectile, Vector3 position, Quaternion rotation)
    //{
    //    projectile.transform.position = position;
    //    projectile.transform.rotation = rotation;
    //}

    //public GameObject GetBullet()
    //{
    //    if (TryGetObject(out GameObject bulletObject))
    //    {
    //        if (bulletObject.TryGetComponent(out Projectile projectile))
    //        {
    //            _projectiles.Add(projectile);
    //            projectile.Impacted += OnImpacted;
    //        }
    //        else
    //        {
    //            throw new NullReferenceException(nameof(projectile));
    //        }

    //        bulletObject.SetActive(true);

    //        return bulletObject;
    //    }

    //    throw new Exception("No active projectile here");
    //}

    //private void OnImpacted(Projectile projectile)
    //{
    //    projectile.gameObject.SetActive(false);
    //}
}
