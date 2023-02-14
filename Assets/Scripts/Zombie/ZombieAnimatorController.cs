using UnityEngine;

public class ZombieAnimatorController : MonoBehaviour
{
    public static class States
    {
        public const string Die = nameof(Die);
        public const string Run = nameof(Run);
        public const string IsAttack = nameof(IsAttack);
    }
}
