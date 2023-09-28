using UnityEngine;

public class ZombieAnimatorController : MonoBehaviour
{
    public static class States
    {
        public const string HeadDie = nameof(HeadDie);
        public const string BodyDie = nameof(BodyDie);
        public const string Run = nameof(Run);
        public const string IsHandAttack = nameof(IsHandAttack);
        public const string IsLegAttack = nameof(IsLegAttack);
        public const string IsMirror = nameof(IsMirror);
    }
}