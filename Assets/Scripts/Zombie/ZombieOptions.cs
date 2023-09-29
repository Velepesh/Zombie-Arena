using System;
using UnityEngine;

[Serializable]
public class ZombieOptions
{
    [SerializeField] private int _award;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _changeTargetDistance;

    public int Award => _award;
    public float MoveSpeed => _moveSpeed;
    public float RotationSpeed => _rotationSpeed;
    public float AttackDistance => _attackDistance;
    public float ChangeTargetDistance => _changeTargetDistance;
}