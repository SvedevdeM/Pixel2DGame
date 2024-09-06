using UnityEngine;
using UnityEngine.AI;

namespace Vices.Scripts.Game
{
    public class RageState : EnemyBaseState
    {
        private Transform _transform;
        private EnemyVision _vision;
        private EnemyDirection _direction;
        private NavMeshAgent _agent;

        private Vector3 _position;

        private float _radiusAttack;
        private float _baseSpeed;
        private float _rageSpeed;

        public RageState(EnemyContext context, EnemyStateFactory factory) : base(context, factory)
        {
            Debug.Log("Rage");
            context.OnStateChange?.Invoke(EnemyStateType.Rage); 
        }

        public override void Execute()
        {
            if (_vision.NearbyTargets.Count <= 0)
            {
                SwitchState(_factory.LastRemember());
                return;
            }
            var point = _vision.NearbyTargets[0].transform.position;
            _position = point;

            _agent.SetDestination(_position);
            _direction.SetTarget(_position);
        }

        public override void CheckSwitch()
        {
            if (_vision.NearbyTargets.Count <= 0) SwitchState(_factory.LastRemember());

            if (Vector2.Distance(_position, _transform.position) < _radiusAttack)
            {
                SwitchState(_factory.Attack());
            }

            if (!_vision.IsTargetSpotted)
            {
                SwitchState(_factory.LastRemember());
            }
        }

        public override void ExitState()
        {
            _agent.speed = _baseSpeed;
            _context.LastPosition = _position;
        }

        public override void EnterState()
        {
            _transform = _context.Transform;
            _agent = _context.Agent;
            _vision = _context.Vision;
            _direction = _context.Rotation;

            _baseSpeed = _context.Speed;
            _rageSpeed = _context.RageSpeed;
            _radiusAttack = _context.RadiusAttack;

            _agent.speed = _rageSpeed;
        }
    }
}