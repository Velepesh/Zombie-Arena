using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Zombie))]
public class MeshChanger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _standartMesh;
    [SerializeField] private SkinnedMeshRenderer _withoutHeadMesh;

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

    private void EnableMesh(SkinnedMeshRenderer meshRenderer)
    {
        meshRenderer.gameObject.SetActive(true);
    }

    private void DisableMesh(SkinnedMeshRenderer meshRenderer)
    {
        meshRenderer.gameObject.SetActive(false);
    }
}