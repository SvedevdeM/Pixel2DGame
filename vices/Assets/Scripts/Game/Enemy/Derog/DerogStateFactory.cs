namespace Vices.Scripts.Game
{
    public class DerogStateFactory : EnemyStateFactory
    {
        public DerogStateFactory(EnemyContext context) : base(context)
        {
        }

        public override EnemyBaseState Idle() => new DerogIdleState(_context, this);
        public override EnemyBaseState Rage() => new DerogRageState(_context, this);
        public override EnemyBaseState Attack() => new DerogAttackState(_context, this);
    }
}
