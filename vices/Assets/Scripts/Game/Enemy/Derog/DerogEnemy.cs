namespace Vices.Scripts.Game
{
    public class DerogEnemy : Enemy
    {
        protected override void Start()
        {
            _context = new EnemyContext(this);
            _states = new DerogStateFactory(_context);

            _currentState = _states.Idle();
            _currentState.EnterState();

            EnemyVision = new EnemyVision(_context);
            Direction = new EnemyDirection(transform, _context);
        }
    }
}