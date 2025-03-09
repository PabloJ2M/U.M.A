using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    [RequireComponent(typeof(PlayerCore))]
    public class Dash : MonoBehaviour
    {
        [SerializeField, Range(0, 30)] private float _speed = 25f;
        [SerializeField, Range(0, 3)] private float _dashDuration = 0.25f, _groundCooldown = 1.5f;

        private PlayerCore _player;
        private WaitUntil _groundCheck;
        private WaitForSeconds _dashTime, _dashCooldown;

        private float _inputDirection;
        private bool _isGrounded, _canDash = true;

        private void Awake() => _player = GetComponent<PlayerCore>();
        private void OnEnable() { _dashTime = new(_dashDuration); _dashCooldown = new(_groundCooldown); _groundCheck = new(() => _isGrounded); }

        private void OnMove(InputValue value) => _inputDirection = value.Get<Vector2>().x;
        private void OnDash() { if (_canDash && _inputDirection != 0) { StopAllCoroutines(); StartCoroutine(DashEffect()); } }
        private void OnCollisionEnter2D(Collision2D collision) => _isGrounded = true;
        private void OnCollisionExit2D(Collision2D collision) => _isGrounded = false;

        private IEnumerator DashEffect()
        {
            _canDash = false;
            _player.DisableMovement();

            _player.RemoveGravity();
            _player.SetVelocity(_inputDirection * _speed, 0f);

            yield return _dashTime;
            _player.ResetGravity();
            _player.EnableMovement();

            yield return _groundCheck;
            yield return _dashCooldown;
            _canDash = true;
        }
    }
}