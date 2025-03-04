using UnityEngine;

namespace Player.Controller
{
    public class Jump : MonoBehaviour
    {
        [SerializeField] private float _jumpForce, _coyoteTime = 0.2f;

        [Header("Detection")]
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _size = 0.5f, _distance = 1;

        private Rigidbody2D _body;
        private float _airTime;
        private bool _canJump;

        private void Awake() => _body = GetComponent<Rigidbody2D>();
        private void Update()
        {
            //ground detection
            bool isGrounded = Physics2D.BoxCast(transform.position, _size * Vector2.one, 0, Vector2.down, _distance, _groundMask);

            //air jump time delay
            bool airJump = _body.linearVelocity.y <= 3f && _airTime < _coyoteTime;
            _airTime = isGrounded ? 0 : _airTime += Time.deltaTime;
            _canJump = isGrounded || airJump;
        }
        private void OnJump()
        {
            if (!_canJump) return;
            _body.linearVelocityY = 0;
            _body.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_distance * Vector3.down + transform.position, _size * Vector2.one);
        }
    }
}