using System.Collections.Generic;
using UnityEngine;

public class BulletCompositeRoot : CompositeRoot
{
    [SerializeField] private List<ProjectilePool> _bulletPools;
    [SerializeField] private List<CasingPool> _casingPools;
    [SerializeField] private List<ImpactPool> _impactPools;

    public override void Compose()
    {
        for (int i = 0; i < _bulletPools.Count; i++)
            _bulletPools[i].GeneratePrefabs();

        for (int i = 0; i < _casingPools.Count; i++)
            _casingPools[i].GeneratePrefabs();

        for (int i = 0; i < _impactPools.Count; i++)
            _impactPools[i].GeneratePrefabs();
    }
}