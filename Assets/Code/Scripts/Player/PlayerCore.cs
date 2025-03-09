using UnityEngine;
using UnityEngine.Events;

namespace Player.Controller
{
    public class PlayerCore : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private UnityEvent<bool> _onMovementStatusChanged;

        public Rigidbody2D Rigidbody2D => _body;
        public Vector2 InverseDirection => new(-transform.localScale.x, 0);
        public float VerticalVelocity => _body.linearVelocityY;
        public float HorizontalVelocity => _body.linearVelocityX;
        public float GravityScale { get; private set; }

        private void Awake() => GravityScale = _body.gravityScale;

        public RaycastHit2D DetectBox(float size, float distance, Vector2 direction, LayerMask mask) =>
            Physics2D.BoxCast(transform.position, Vector2.one * size, 0, direction, distance, mask);
        public RaycastHit2D DetectRay(float distance, Vector2 direction, LayerMask mask) =>
            Physics2D.Raycast(transform.position, direction, distance, mask);
        public RaycastHit2D DetectCircle(float radius, float distance, Vector2 direction, LayerMask mask) =>
            Physics2D.CircleCast(transform.position, radius, direction, distance, mask);

        public void RemoveGravity() => _body.gravityScale = 0;
        public void ResetGravity() => _body.gravityScale = GravityScale;
        public void Freeze() => _body.constraints = RigidbodyConstraints2D.FreezeAll;
        public void UnFreeze() => _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        public void AddForce(Vector2 direction, ForceMode2D force = ForceMode2D.Impulse) => _body.AddForce(direction, force);
        public void MoveToTarget(Vector2 target, float speed) => _body.position = Vector2.MoveTowards(_body.position, target, speed);

        public void SetVelocity(Vector2 value) => _body.linearVelocity = value;
        public void SetVelocity(float x, float y) => _body.linearVelocity = new(x, y);
        public void StopVerticalVelocity() => _body.linearVelocityY = 0;
        public void ChangeDirection() => transform.localScale = new(-transform.localScale.x, 1, 1);

        public void DisableMovement() => _onMovementStatusChanged.Invoke(false);
        public void EnableMovement() => _onMovementStatusChanged.Invoke(true);
    }
}