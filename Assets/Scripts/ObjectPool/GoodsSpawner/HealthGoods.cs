using System;
using UnityEngine;

public class HealthGoods : Goods, IHealthGoods
{
    [SerializeField] private int _minHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Transform _model;
    [SerializeField] private float _rotationTime;

    private ISpawnPoint _spawnPoint;

    private Health _twinsHealth;
    private int _extraHealth;

    public event Action<int> Inited;
    public override event Action<Goods> PickedUp;

    private void OnValidate()
    {
        _minHealth = Mathf.Clamp(_minHealth, 1, _maxHealth);
        _maxHealth = Mathf.Clamp(_maxHealth, _minHealth, int.MaxValue);
        _rotationTime = Mathf.Clamp(_rotationTime, 0f, float.MaxValue);
    }

    public void SetTwinsHealth(Health health)
    {
        if (health == null)
            throw new ArgumentNullException(nameof(health));

        _twinsHealth = health;
    }

    public override void Init(ISpawnPoint spawnPoint)
    {
        if(spawnPoint == null)
            throw new ArgumentNullException(nameof(spawnPoint));

        _spawnPoint = spawnPoint;
        _spawnPoint.TakePoint();
        _extraHealth = UnityEngine.Random.Range(_minHealth, _maxHealth);
        Rotate(_rotationTime);
        Inited?.Invoke(_extraHealth);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            ReplenishHealth(player);
            _spawnPoint.ResetPoint();

            PickedUp?.Invoke(this);
        }
    }

    protected override void Rotate(float durationTime)
    {
        ObjectRotator.RotateInfinity(_model, new Vector3(0f, 360f, 0f), durationTime);
    }

    private void ReplenishHealth(Player player)
    {
        _twinsHealth.ReplenishHealth(_extraHealth);
        player.Health.ReplenishHealth(_extraHealth);
    }
}

public interface IHealthGoods
{
    void SetTwinsHealth(Health health);
}