using UnityEngine;

namespace Vices.Scripts.Game
{
    public class EnemyDirection
    {
        private EnemyContext _context;
        private Transform _transform;
        private Vector3 _position;
        private EnemyDirectionType _direction;

        public EnemyDirection(Transform transform, EnemyContext enemyContext)
        {
            _context = enemyContext;
            _transform = transform;
        }

        public void Execute()
        {

        }

        public void SetTarget(Vector3 position)
        {
            _position = position;

            var directionVector = (_position - _transform.position);
            directionVector.Normalize();
            var signY = Mathf.Sign(directionVector.y);
            var signX = Mathf.Sign(directionVector.x);
            directionVector.y = signY * directionVector.y < 0.2 ? 0 : signY * 1;
            directionVector.x = signX * directionVector.x < 0.2 ? 0 : signX * 1;

            if (directionVector.y == -1) _direction = EnemyDirectionType.Up;
            if (directionVector.y == 1) _direction = EnemyDirectionType.Down;
            if (directionVector.x == 1) _direction = EnemyDirectionType.SideRight;
            if (directionVector.x == -1) _direction = EnemyDirectionType.SideLeft;

            _context.OnDirectionChange?.Invoke(_direction);
        }
    }
}