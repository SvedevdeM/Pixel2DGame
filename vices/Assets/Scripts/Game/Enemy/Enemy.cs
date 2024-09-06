using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vices.Scripts.Game
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected EnemyParameters _parameters;
        [SerializeField] protected List<Transform> _points;

        protected EnemyStateFactory _states;
        protected EnemyContext _context;
        protected EnemyBaseState _currentState;

        public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public EnemyVision EnemyVision { get; protected set; }
        public EnemyDirection Direction { get; protected set; }
        public EnemyParameters Parameters => _parameters;
        public EnemyContext Context => _context;
        public List<Transform> Points => _points;

        protected virtual void Start()
        {
            _context = new EnemyContext(this);
            _states = new EnemyStateFactory(_context);

            EnemyVision = new EnemyVision(_context);
            Direction = new EnemyDirection(transform, _context);

            _currentState = _states.Idle();
            _currentState.EnterState();
        }

        private void Update()
        {
            _currentState.Execute();
            _currentState.CheckSwitch();

            EnemyVision.Execute();
        }

        public void SubscribeOnChangeDirectionChange(Action<EnemyDirectionType> method)
        {
            Context.OnDirectionChange += method;
        }

        public void SubscribeOnChangeStateChange(Action<EnemyStateType> method)
        {
            Context.OnStateChange += method;
        }
    }
}