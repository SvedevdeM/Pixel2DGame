namespace Vices.Scripts.Game
{
    public class EnemyStateFactory
    {
        protected EnemyContext _context;

        public EnemyStateFactory(EnemyContext context)
        {
            _context = context;
        }

        public virtual EnemyBaseState Idle() => new IdleState(_context, this);
        public virtual EnemyBaseState LastRemember() => new LastRememberState(_context, this);
        public virtual EnemyBaseState Patrool() => new PatroolState(_context, this);
        public virtual EnemyBaseState Attack() => new AttackState(_context, this);
        public virtual EnemyBaseState Rage() => new RageState(_context, this);
    }
}