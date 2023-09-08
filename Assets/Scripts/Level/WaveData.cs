using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Wave", order = 51)]
public class WaveData : ScriptableObject
{
    [SerializeField] private List<Zombie> _templates;
    [SerializeField] private float _delay;

    public int Count => _templates.Count;
}