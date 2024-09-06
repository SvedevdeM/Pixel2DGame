using UnityEngine;
using UnityEngine.AI;

namespace Vices.Scripts.Game
{
    public class DerogRageState : EnemyBaseState
    {
        private EnemyDirection _direction;
        private NavMeshAgent _agent;
        private Transform _player;
        private Transform _transform;
        private Vector3 _position;
        private float _radiusAttack;

        public DerogRageState(EnemyContext context, EnemyStateFactory factory) : base(context, factory)
        {
            Debug.Log("Rage");
            context.OnStateChange?.Invoke(EnemyStateType.Rage);
            context.OnDirectionChange?.Invoke(EnemyDirectionType.SideRight);
        }

        public override void CheckSwitch()
        {
            if (Vector2.Distance(_position, _transform.position) < _radiusAttack)
            {
                SwitchState(_factory.Attack());
            }
        }

        public override void Execute()
        {
            var point = _player.position;
            _position = point;

            _agent.SetDestination(_position);
            _direction.SetTarget(_position);
        }

        public override void EnterState()
        {
            _player = GameObject.FindObjectOfType<PlayerStateManager>().transform;

            _transform = _context.Transform;
            _agent = _context.Agent;
            _direction = _context.Rotation;
            _radiusAttack = _context.RadiusAttack;

            _agent.speed = _context.RageSpeed;
        }

        public override void ExitState() { }
    }
}
