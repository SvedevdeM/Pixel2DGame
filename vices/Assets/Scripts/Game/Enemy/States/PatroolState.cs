using UnityEngine;
using UnityEngine.AI;

namespace Vices.Scripts.Game
{
    public class PatroolState : EnemyBaseState
    {
        private EnemyDirection _rotation;
        private NavMeshAgent _agent;
        private Transform[] _points;
        private int _index;

        public PatroolState(EnemyContext context, EnemyStateFactory factory) : base(context, factory)
        {
            Debug.Log("Patrool");
        }

        public override void Execute()
        {
            if (_agent.remainingDistance <= 0.25f)
            {
                _index++;
                if (_index >= _points.Length) _index = 0;
                _agent.SetDestination(_points[_index].position);
                _rotation.SetTarget(_points[_index].position);
            }
        }

        public override void CheckSwitch()
        {
            if (_context.Vision.IsTargetSpotted)
            {
                SwitchState(_factory.Rage());
            }
        }

        public override void ExitState()
        {
            _context.Index = _index;
        }

        public override void EnterState()
        {
            _agent = _context.Agent;
            _rotation = _context.Rotation;
            _points = _context.PatrolPoints.ToArray();
            _index = _context.Index;

            _agent.SetDestination(_points[_index].position);
            _rotation.SetTarget(_points[_index].position);

            _context.OnStateChange?.Invoke(EnemyStateType.Patrool);
        }
    }
}