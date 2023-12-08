using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObjectPool : MonoCache
{
    [SerializeField] private Transform _container;

    private List<GameObject> _pool = new List<GameObject>();
    
    public abstract void GeneratePrefabs();

    protected GameObject SpawnPrefab(GameObject prefab)
    {
        GameObject spawned = Instantiate(prefab, _container.position, Quaternion.identity);
        spawned.transform.SetParent(_container);
        spawned.SetActive(false);

        _pool.Add(spawned);

        return spawned;
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(p => p.activeSelf == false);

        return result != null;
    }

    protected bool TryGetRandomObject(out GameObject result)
    {
        List<GameObject> disabledObjectsList = new List<GameObject>();

        for (int i = 0; i < _pool.Count; i++)
        {
            GameObject go = _pool[i];
            if (go.activeSelf == false)
                disabledObjectsList.Add(go);
        }

        int randomIndex = Random.Range(0, disabledObjectsList.Count);

        result = disabledObjectsList[randomIndex];
        result.SetActive(true);

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