using UnityEngine;

namespace Player.Controller
{
    [RequireComponent(typeof(PlayerCore))]
    public class Jump : MonoBehaviour
    {
        [SerializeField] private float _jumpForce, _coyoteTime = 0.2f;

        [Header("Detection")]
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _size = 0.5f, _distance = 1;

        private PlayerCore _player;
        private float _airTime;
        private bool _canJump;

        private void Awake() => _player = GetComponent<PlayerCore>();
        private void Update()
        {
            //ground detection
            bool isGrounded = _player.DetectBox(_size, _distance, Vector2.down, _groundMask).collider != null;

            //air jump time delay
            bool airJump = _player.VerticalVelocity <= _player.GravityScale && _airTime < _coyoteTime;
            _airTime = isGrounded ? 0 : _airTime += Time.deltaTime;
            _canJump = isGrounded || airJump;
        }
        private void OnJump()
        {
            if (!_canJump) return;
            _player.StopVerticalVelocity();
            _player.AddForce(_jumpForce * Vector2.up);
        }
        public void ResetAirTime() => _airTime = 0;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + (_distance * Vector3.down), _size * Vector2.one);
        }
    }
}