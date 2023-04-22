using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Zombie))]
public class MeshChanger : MonoBehaviour
{
    [SerializeField] private List<SkinnedMeshRenderer> _standartMesh;
    [SerializeField] private List<SkinnedMeshRenderer> _withoutHeadMesh;

    private Zombie _zombie;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        _zombie.HeadKilled += OnHeadKilled;
    }

    private void OnDisable()
    {
        _zombie.HeadKilled -= OnHeadKilled;
    }


    public void WearStandartMesh()
    {
        DisableMesh(_withoutHeadMesh);
        EnableMesh(_standartMesh);
    }

    private void OnHeadKilled() => WearWithoutHeadMesh();

    private void WearWithoutHeadMesh()
    {
        DisableMesh(_standartMesh);
        EnableMesh(_withoutHeadMesh);
    }

    private void EnableMesh(List<SkinnedMeshRenderer> meshRenderer)
    {
        for (int i = 0; i < meshRenderer.Count; i++)
            meshRenderer[i].gameObject.SetActive(true);
    }

    private void DisableMesh(List<SkinnedMeshRenderer> meshRenderer)
    {
        for (int i = 0; i < meshRenderer.Count; i++)
            meshRenderer[i].gameObject.SetActive(false);
    }
}