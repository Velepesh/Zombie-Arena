using System;
using UnityEngine;

[Serializable]
public class ZombieOptions
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackDistance;

    public float MoveSpeed => _moveSpeed;
    public float AttackDistance => _attackDistance;
}