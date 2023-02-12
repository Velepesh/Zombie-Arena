using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObjectPool : MonoCache
{
    [SerializeField] private GameObject _container;

    private List<GameObject> _pool = new List<GameObject>();

    public abstract void StartGenerate();

    public void DisableAllObjects()
    {
        for (int i = 0; i < _pool.Count; i++)
            DisableObject(_pool[i]);
    }

    protected void Init(GameObject prefab)
    {
        GameObject spawned = Instantiate(prefab, _container.transform);
        spawned.SetActive(false);

        _pool.Add(spawned);
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(p => p.activeSelf == false);

        return result != null;
    }

    protected GameObject GetFirstObject()
    {
        return _pool.FirstOrDefault();
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
        //if (result.activeSelf == true)
        //    result.SetActive(false);
        _pool.Remove(result);
        Destroy(result);
    }

    public void ResetPool()
    {
        foreach (var item in _pool)
            item.SetActive(false);
    }

    protected void Shuffle()
    {
        for (int i = _pool.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            GameObject temp = _pool[j];
            _pool[j] = _pool[i];
            _pool[i] = temp;
        }
    }
}
