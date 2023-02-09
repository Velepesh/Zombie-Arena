using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class MeshChanger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _currentMesh;
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

    private void OnHeadKilled()
    {
        _currentMesh.gameObject.SetActive(false);
        _withoutHeadMesh.gameObject.SetActive(true);
    }
}