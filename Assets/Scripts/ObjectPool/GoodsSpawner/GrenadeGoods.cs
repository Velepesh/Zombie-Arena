using InfimaGames.LowPolyShooterPack;
using System;
using UnityEngine;

public class GrenadeGoods : Goods
{
    [SerializeField] private Transform _model;
    [SerializeField] private float _rotationTime;

    private ISpawnPoint _spawnPoint;

    public override event Action<Goods> PickedUp;

    public override void Init(ISpawnPoint spawnPoint)
    {
        if (spawnPoint == null)
            throw new ArgumentNullException(nameof(spawnPoint));

        _spawnPoint = spawnPoint;
        _spawnPoint.TakePoint();

        Rotate(_rotationTime);
    }

    private void OnValidate()
    {
        _rotationTime = Mathf.Clamp(_rotationTime, 0f, float.MaxValue);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Character character))
        {
            character.AddGrenade();
            _spawnPoint.ResetPoint();
            PickedUp?.Invoke(this);
        }
    }

    protected override void Rotate(float durationTime)
    {
        ObjectRotator.RotateInfinity(_model, new Vector3(0f, 0f, 360f), durationTime);
    }
}