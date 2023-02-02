using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObjectPool : MonoCache
{
    [SerializeField] private GameObject _container;
    [SerializeField] private int _numberOfEveryTemplate;

    private List<GameObject> _pool = new List<GameObject>();

    private void OnValidate()
    {
        _numberOfEveryTemplate = Mathf.Clamp(_numberOfEveryTemplate, 0, int.MaxValue);
    }

    public abstract void StartGenerate();

    public void DisableAllObjects()
    {
        for (int i = 0; i < _pool.Count; i++)
            DisableObject(_pool[i]);
    }

    protected void Init(GameObject prefab)
    {
        for (int i = 0; i < _numberOfEveryTemplate; i++)
        {
            GameObject spawned = Instantiate(prefab, _container.transform);
            spawned.SetActive(false);

            _pool.Add(spawned);
        }
    }

    protected bool TryGetObject(out GameObject result, ZombieType type)
    {
        result = _pool.FirstOrDefault(p => p.activeSelf == false && p.GetComponent<Zombie>().Type == type);

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
        if (result.activeSelf == true)
            result.SetActive(false);
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
