using UnityEngine;

namespace Vices.Scripts.Game
{
    public class DerogAttackState : EnemyBaseState
    {
        private float _cooldownTime;
        private bool _switchState;

        public DerogAttackState(EnemyContext context, EnemyStateFactory factory) : base(context, factory)
        {
            Debug.Log("Attack");
            context.OnStateChange?.Invoke(EnemyStateType.Attack);
        }

        public override void Execute()
        {
            _cooldownTime -= Time.deltaTime;
            
            if (_cooldownTime < 0)
            {
                _switchState = true;
            }
        }

        public override void CheckSwitch()
        {
            if (!_switchState) return;

            SwitchState(_factory.Rage());
        }

        public override void EnterState()
        {
            _cooldownTime = _context.AttackCooldown;
            var colliders = Physics.OverlapBox(_context.Transform.position, _context.AttackCollider);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].TryGetComponent<Health>(out var health)) continue;
                health.DealDamage(_context.AttackDamage);
            }
            _context.Agent.speed = 0;
        }

        public override void ExitState()
        {
            _context.Agent.speed = _context.Speed;
        }
    }
}
