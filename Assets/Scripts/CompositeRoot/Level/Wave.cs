using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Level/Wave", order = 51)]
public class Wave : ScriptableObject
{
    [SerializeField] private List<Zombie> _templates;
    [SerializeField] private int _maxActibeZombieDesktop;
    [SerializeField] private int _maxActiveZombieMobile;
    [SerializeField] private float _delay;

    public int Count => _templates.Count;
    public float Delay => _delay;

    public Zombie GetTemplate(int index)
    {
        return _templates[index];
    }
    private void OnValidate()
    {
        _maxActibeZombieDesktop = Math.Clamp(_maxActibeZombieDesktop, 1, _templates.Count);
        _maxActiveZombieMobile = Math.Clamp(_maxActiveZombieMobile, 1, _templates.Count);
    }
    public int GetMaxActiveZombie(bool isMobile)
    {
        if (isMobile)
            return _maxActiveZombieMobile;
        else
            return _maxActibeZombieDesktop;
    }
}