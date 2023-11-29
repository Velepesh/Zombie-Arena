using UnityEngine;

[CreateAssetMenu(fileName = "Classic Level ", menuName = "Level/Classic Level", order = 51)]
public class ClassicLevel : Level, ILevel
{
    public int WavesCount => Waves.Count;
}