using UnityEngine;

public class GoodsEffect : MonoBehaviour
{
    [SerializeField] private Goods _goods;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ParticleSystem _pickedUpEffect;

    private void OnEnable()
    {
        _goods.PickedUp += OnPickedUp;
    }

    private void OnDisable()
    {
        _goods.PickedUp -= OnPickedUp;
    }

    private void OnPickedUp(Goods goods)
    {
        Instantiate(_pickedUpEffect.gameObject, _spawnPoint.position, Quaternion.identity);
    }
}