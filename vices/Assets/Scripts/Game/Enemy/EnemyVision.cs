using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vices.Scripts.Game
{
    public class EnemyVision
    {
        private Transform _transform;

        private float _senseRadius;
        private float _radiusLook;
        private float _radiusListen;
        private float _viewAngle;

        private LayerMask _obstacleMask, _detectionMask;

        private Collider[] _targetsInRadius;

        public List<Collider> NearbyTargets { get; private set; }
        public bool IsTargetSpotted { get; private set; }

        public EnemyVision(EnemyContext context)
        {
            _transform = context.Transform;

            _senseRadius = context.RadiusSence;
            _radiusLook = context.RadiusLook;
            _radiusListen = context.RadiusListen;
            _viewAngle = context.AngleView;

            _obstacleMask = context.ObstacleLayer;
            _detectionMask = context.DetectionLayer;

            NearbyTargets = new List<Collider>();
        }

        public void Execute()
        {
            NearbyTargets.Clear();

            IsTargetSpotted = Sense() || Look() || Listen(); 
        }

        private bool Sense()
        {
            var overlapColliders = Physics.OverlapSphere(_transform.position, _senseRadius);
            if (overlapColliders.Length <= 0) return false;
            for (int i = 0; i < overlapColliders.Length; i++)
            {
                if (!overlapColliders[i].CompareTag("Player")) continue;
                if (!TryEmitRay(overlapColliders[i], out var raycastHit)) continue;

                NearbyTargets.Add(overlapColliders[i]);
                return true;
            }

            return false;
        }

        private bool Look()
        {
            _targetsInRadius = Physics.OverlapSphere(_transform.position, _radiusLook, _detectionMask);

            NearbyTargets.Clear();

            for (int i = 0; i < _targetsInRadius.Length; i++)
            {
                Transform target = _targetsInRadius[i].transform;

                Vector2 dirTarget = new Vector2(target.position.x - _transform.position.x, target.position.y - _transform.position.y);
                Vector2 dir = _transform.right;

                if (Vector2.Angle(dirTarget, dir) < _viewAngle / 2)
                {
                    float distanceTarget = Vector2.Distance(_transform.position, target.position);

                    if (Physics.Raycast(_transform.position, dirTarget, distanceTarget, _obstacleMask))
                    {
                        NearbyTargets.Add(_targetsInRadius[i]);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool Listen()
        {
            var overlapColliders = Physics.OverlapSphere(_transform.position, _radiusListen);
            if (overlapColliders.Length <= 0) return false;
            for (int i = 0; i < overlapColliders.Length; i++)
            {
                if (!overlapColliders[i].TryGetComponent<Sound>(out var sound)) continue;
                if (!TryEmitRay(overlapColliders[i], out var raycastHit)) continue;

                NearbyTargets.Add(overlapColliders[i]);
                return true;
            }

            return false;
        }

        private bool TryEmitRay(Collider collider, out RaycastHit hit)
        {
            Debug.DrawRay(_transform.position, collider.transform.position - _transform.position);
            return Physics.Raycast(_transform.position, collider.transform.position - _transform.position, out hit);
        }
    }
}