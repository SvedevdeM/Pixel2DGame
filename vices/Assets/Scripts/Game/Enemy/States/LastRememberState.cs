using UnityEngine;
using UnityEngine.AI;

namespace Vices.Scripts.Game
{
    public class LastRememberState : EnemyBaseState
    {
        private NavMeshAgent _agent;
        private EnemyVision _vision;
        private EnemyDirection _rotation;
        private Vector3 _lastRememberedPosition;

        private bool _wentToLastPoint;
        private float _findingTime;

        public LastRememberState(EnemyContext context, EnemyStateFactory factory) : base(context, factory) 
        {
            Debug.Log("LastRemember");
        }

        public override void Execute()
        {
            if (_wentToLastPoint) _findingTime -= Time.deltaTime;

            if (_agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
            {
                Vector3 randomPoint = new Vector3(UnityEngine.Random.Range(_lastRememberedPosition.x - 1f, _lastRememberedPosition.x + 1f), UnityEngine.Random.Range(_lastRememberedPosition.y - 1f, _lastRememberedPosition.y + 1f));
                _agent.SetDestination(randomPoint);
                _rotation.SetTarget(randomPoint);
                _wentToLastPoint = true;
            }
        }

        public override void CheckSwitch()
        {
            if (_vision.IsTargetSpotted) SwitchState(_factory.Rage());
            if(_findingTime <= 0) SwitchState(_factory.Idle());
        }

        public override void EnterState()
        {
            _agent = _context.Agent;
            _vision = _context.Vision;
            _rotation = _context.Rotation;

            _findingTime = _context.FindingTime + 2f;
            _lastRememberedPosition = _context.LastPosition;

            _agent.SetDestination(_lastRememberedPosition);
            _rotation.SetTarget(_lastRememberedPosition);

            _context.OnStateChange?.Invoke(EnemyStateType.LastRemembered);
        }

        public override void ExitState()
        {

        }
    }
}

