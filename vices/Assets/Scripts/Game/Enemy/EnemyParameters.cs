using UnityEngine;

namespace Vices.Scripts.Game
{
    [CreateAssetMenu(fileName = nameof(EnemyParameters), menuName = "JabberwockyWorld/Enemy/" + nameof(EnemyParameters))]
    public class EnemyParameters : ScriptableObject
    {   
        [Tooltip("”гол обзора противника")]
        public int AngleView;

        [Header("Sense Settings")]
        public float RadiusSniffing = 1.0f;
        public float RadiusLook = 3.0f;
        public float RadiusListen = 2.0f;
        public float RadiusAttack = 1.2f;
        public LayerMask ObstacleLayer;
        public LayerMask DetectionLayer;

        [Header("State Settings")]
        public float RageSpeed = 1.5f;
        public float Speed = 1f;
        public float AttackCooldown = 1.3f;
        public float AttackDamage = 5.0f;
        public Vector3 AttackCollider = new Vector3(1f, 1f, 1f);

        [Header("Stay Settings")]
        public bool StandingStill;

        [Header("Find Settings")]
        public float FindingTime = 3f;
    }
}
