using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    [RequireComponent(typeof(PlayerCore))]
    public class MagneticField : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _magnetAcceleration = 2f;
        [SerializeField, Range(0, 1)] private float _offset = 0.5f;
        [SerializeField, Range(0, 50)] private float _speed = 1f;

        [SerializeField] private LayerMask _wallMask;
        [SerializeField, Range(0, 10)] private float _distance = 10f;

        [SerializeField] private UnityEvent _onRelease;

        private PlayerCore _player;
        private RaycastHit2D _wallSurface;
        private float _inputDirection;
        private bool _isGrabed;

        private void Awake() => _player = GetComponent<PlayerCore>();
        private void OnMove(InputValue value) => _inputDirection = value.Get<Vector2>().x;

        private void Update()
        {
            _wallSurface = _player.DetectRay(_distance, Vector2.right * _inputDirection, _wallMask);
            if (_wallSurface.collider == null || _inputDirection == 0) DisableEffect(); else EnableEffect();
        }
        private void EnableEffect()
        {
            if (_isGrabed) return;

            _isGrabed = true;
            _player.Freeze();
            _player.DisableMovement();
            StartCoroutine(GrabEffect());
        }
        private void DisableEffect()
        {
            if (!_isGrabed) return;

            _isGrabed = false;
            _player.UnFreeze();
            _player.EnableMovement();
            _player.AddForce(Vector2.up);
            _onRelease.Invoke();
            StopAllCoroutines();
        }

        private IEnumerator GrabEffect()
        {
            float startPosition = _player.Rigidbody2D.position.x;
            Vector2 targetPosition = _wallSurface.point + (_wallSurface.normal * _offset);
            float totalDistance = _wallSurface.distance - _offset;
            float distancePercent = Distance() / totalDistance;

            while (distancePercent < 1)
            {
                yield return null;
                float factor = Mathf.Pow(distancePercent, _magnetAcceleration);
                _player.MoveToTarget(targetPosition, _speed * Mathf.Max(factor, 0.1f) * Time.deltaTime * 10f);

                distancePercent = Distance() / totalDistance;
            }

            _player.Rigidbody2D.position = targetPosition;
            float Distance() => Mathf.Abs(_player.Rigidbody2D.position.x - startPosition) + 0.01f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _distance);
        }
    }
}