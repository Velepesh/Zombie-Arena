using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Level/Wave", order = 51)]
public class Wave : ScriptableObject
{
    [SerializeField] private List<Zombie> _templates;
    [SerializeField] private int _maxActiveZombie;
    [SerializeField] private float _delay;

    public int MaxActiveZombie => _maxActiveZombie;
    public int Count => _templates.Count;
    public float Delay => _delay;

    public Zombie GetTemplate(int index)
    {
        return _templates[index];
    }
}