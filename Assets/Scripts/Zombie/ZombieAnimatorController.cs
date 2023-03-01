using UnityEngine;

public class ZombieAnimatorController : MonoBehaviour
{
    public static class States
    {
        public const string HeadDie = nameof(HeadDie);
        public const string BodyDie = nameof(BodyDie);
        public const string Run = nameof(Run);
        public const string IsAttack = nameof(IsAttack);
        public const string IsMirror= nameof(IsMirror);
    }
}
