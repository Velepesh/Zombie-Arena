using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level_", menuName = "Level/Level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private List<Wave> _waves;

    public int WavesCount => _waves.Count;

    public Wave GetWave(int index)
    {
        return _waves[index];
    }
}