using UnityEngine;

public class Impact : MonoBehaviour 
{
    [SerializeField] private ImpactType _type;

    public ImpactType Type => _type;
}

public enum ImpactType
{
    Blood,
    Metal,
    Dirt,
    Concrete,
    Target,
    ExplosiveBarrel,
}