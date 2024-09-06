using UnityEngine;

namespace Vices.Scripts.Game
{
    public class IdleState : EnemyBaseState
    {
        public IdleState(EnemyContext context, EnemyStateFactory factory) : base(context, factory)
        {
            Debug.Log("Idle");
            context.OnStateChange?.Invoke(EnemyStateType.Idle);
        }

        public override void CheckSwitch()
        {
            if (_context.Vision.IsTargetSpotted)
            {
                SwitchState(_factory.Rage());
            }
            else if (!_context.IsStand)
            {
                SwitchState(_factory.Patrool());
            }
        }

        public override void EnterState()
        {
        }

        public override void ExitState()
        {

        }

        public override void Execute()
        {
        }
    }
}