using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Level/Wave", order = 51)]
public class Wave : ScriptableObject, IWave
{
    [SerializeField] private List<Zombie> _templates;
    [SerializeField] private int _maxActiveZombieDesktop;
    [SerializeField] private float _delay;

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
            int maxMobileCount = _maxActiveZombieDesktop;

            if (_templates.Count > 8)
                maxMobileCount = _maxActiveZombieDesktop - 2;
            else if (_templates.Count > 4)
                maxMobileCount = _maxActiveZombieDesktop - 1;

            return maxMobileCount;
        }
        else 
        {
            return _maxActiveZombieDesktop;
        }
    }
}

public interface IWave
{
    public int Count { get; }
    public float Delay { get; }
}