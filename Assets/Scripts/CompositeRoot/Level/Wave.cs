using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Level/Wave", order = 51)]
public class Wave : ScriptableObject
{
    [SerializeField] private List<Zombie> _templates;
    [SerializeField] private int _maxActiveZombieDesktop;
    [SerializeField] private float _delay;

    private int _maxActiveZombieMobile;

    public int Count => _templates.Count;
    public float Delay => _delay;

    private void OnValidate()
    {
        _maxActiveZombieDesktop = Math.Clamp(_maxActiveZombieDesktop, 1, _templates.Count);

        _delay = Math.Clamp(_delay, 0f, 2f);
    }

    public Zombie GetTemplate(int index)
    {
        return _templates[index];
    }

    public int GetMaxActiveZombie(bool isMobile)
    {
        return GetMaxCount(isMobile);
    }

    private int GetMaxCount(bool isMobile)
    {
        if (isMobile)
        {
            if (_templates.Count > 8)
                _maxActiveZombieMobile = _maxActiveZombieDesktop - 2;
            else if (_templates.Count > 5)
                _maxActiveZombieMobile = _maxActiveZombieDesktop - 1;
            else
                _maxActiveZombieMobile = _maxActiveZombieDesktop;

            return _maxActiveZombieMobile;
        }
        else 
        {
            return _maxActiveZombieDesktop;
        }
    }
}