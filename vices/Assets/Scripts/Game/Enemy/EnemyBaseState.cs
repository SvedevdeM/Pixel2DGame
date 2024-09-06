namespace Vices.Scripts.Game
{
    public abstract class EnemyBaseState
    {
        protected EnemyContext _context;
        protected EnemyStateFactory _factory;

        public EnemyBaseState(EnemyContext context, EnemyStateFactory factory)
        {
            _context = context;
            _factory = factory;
        }

        protected void SwitchState(EnemyBaseState state)
        {
            _context.CurrentState.ExitState();
            _context.CurrentState = state;
            _context.CurrentState.EnterState();
        }

        public abstract void EnterState();
        public abstract void Execute();
        public abstract void ExitState();
        public abstract void CheckSwitch();
    }
}