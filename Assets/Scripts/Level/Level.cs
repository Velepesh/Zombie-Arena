using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "Level/Level", order = 51)]
public class Level : ScriptableObject
{
    [SerializeField] private List<Wave> _waves;
}
