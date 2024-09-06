using UnityEngine;

namespace Vices.Scripts.Game
{
    public class DerogIdleState : EnemyBaseState
    {
        public DerogIdleState(EnemyContext context, EnemyStateFactory factory) : base(context, factory)
        {
            Debug.Log("Idle");
            context.OnStateChange?.Invoke(EnemyStateType.Idle);
        }

        public override void CheckSwitch()
        {
            if (_context.IsStand)
            {
                SwitchState(_factory.Rage());
            }
        }

        public override void EnterState() { }

        public override void Execute() { }

        public override void ExitState() { }
    }
}
