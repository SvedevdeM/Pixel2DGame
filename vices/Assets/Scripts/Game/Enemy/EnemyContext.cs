using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Vices.Scripts.Game
{
    public class EnemyContext
    {
        private Enemy _enemy;

        public Action<EnemyDirectionType> OnDirectionChange;
        public Action<EnemyStateType> OnStateChange;

        public EnemyContext(Enemy enemy)
        {
            _enemy = enemy;
            var parameters = enemy.Parameters;

            Transform = enemy.transform;
            PatrolPoints = _enemy.Points;
            Agent = enemy.GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;

            AngleView = parameters.AngleView;
            RadiusSence = parameters.RadiusSniffing;
            RadiusLook = enemy.Parameters.RadiusLook;
            RadiusListen = parameters.RadiusListen;
            RadiusAttack = parameters.RadiusAttack;
            RageSpeed = parameters.RageSpeed;

            Speed = parameters.Speed;

            ObstacleLayer = parameters.ObstacleLayer;
            DetectionLayer = parameters.DetectionLayer;

            AttackCollider = parameters.AttackCollider;
            AttackCooldown = parameters.AttackCooldown;
            AttackDamage = parameters.AttackDamage;
            FindingTime = parameters.FindingTime;
            IsStand = parameters.StandingStill;
        }

        public Enemy Enemy => _enemy;
        public EnemyVision Vision => _enemy.EnemyVision;
        public EnemyDirection Rotation => _enemy.Direction;
        public NavMeshAgent Agent { get; private set; }
        public Transform Transform { get; private set; }
        public Vector3 Direction { get; private set; }
        public Vector3 LastPosition { get; set; }
        public Vector3 AttackCollider { get; set; }
        public LayerMask ObstacleLayer { get; private set; }
        public LayerMask DetectionLayer { get; private set; }
        public List<Transform> PatrolPoints { get; private set; }

        public int AngleView { get; private set; }
        public float RadiusSence { get; private set; }
        public float FindingTime { get; private set; }
        public float RadiusLook { get; private set; }
        public float RadiusListen { get; private set; }
        public float RadiusAttack { get; private set; }
        public float RageSpeed { get; private set; }
        public float Speed { get; private set; }
        public float AttackCooldown { get; private set; }
        public float AttackDamage { get; private set; }
        public bool IsStand { get; set; }
        public int Index { get; set; }
        public EnemyDirectionType DirectionType { get; set; }
        public EnemyBaseState CurrentState { get { return _enemy.CurrentState; } set { _enemy.CurrentState = value; } }
    }
}
