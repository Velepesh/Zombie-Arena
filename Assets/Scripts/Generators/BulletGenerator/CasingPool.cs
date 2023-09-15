using InfimaGames.LowPolyShooterPack.Legacy;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CasingPool : ObjectPool
{
    [SerializeField] private Casing _casing;
    [SerializeField] private int _count;

    private List<Casing> _casings = new List<Casing>();

    private void OnValidate()
    {
        _count = Mathf.Clamp(_count, 0, int.MaxValue);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _casings.Count; i++)
            _casings[i].Disabled -= OnDisabled;
    }

    public override void GeneratePrefabs()
    {
        for (int i = 0; i < _count; i++)
            SpawnPrefab(_casing.gameObject);
    }

    public void SetCasingTransform(GameObject casing, Vector3 position, Quaternion rotation)
    {
        casing.transform.position = position;
        casing.transform.rotation = rotation;
    }

    public GameObject GetCasing()
    {
        if (TryGetObject(out GameObject casingObject))
        {
            if (casingObject.TryGetComponent(out Casing casing))
            {
                _casings.Add(casing);
                casing.Disabled += OnDisabled;
            }
            else
            {
                throw new NullReferenceException(nameof(casing));
            }

            casingObject.SetActive(true);

            return casingObject;
        }

        throw new Exception("No active casing here");
    }

    private void OnDisabled(Casing casing)
    {
        _casings.Remove(casing);
        casing.gameObject.SetActive(false);
    }
}