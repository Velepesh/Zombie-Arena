using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObjectPool : MonoCache
{
    [SerializeField] private GameObject _container;

    private List<GameObject> _pool = new List<GameObject>();

    public abstract void StartGenerate();

    protected void SpawnPrefab(GameObject prefab)
    {
        GameObject spawned = Instantiate(prefab, _container.transform.position, Quaternion.identity);
        spawned.transform.SetParent(_container.transform);
        spawned.SetActive(false);

        _pool.Add(spawned);
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(p => p.activeSelf == false);

        return result != null;
    }


    protected void DisableObject(Vector3 disablePosition)
    {
        foreach (var item in _pool)
        {
            if (item.activeSelf == true)
                if (item.transform.position == disablePosition)
                    item.SetActive(false);
        }
    }

    protected void DisableObject(GameObject result)
    {
        _pool.Remove(result);
        Destroy(result);
    }
}