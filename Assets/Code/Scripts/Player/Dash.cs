using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    public class Dash : MonoBehaviour
    {
        [SerializeField, Range(0, 30)] private float _speed = 25f;
        [SerializeField, Range(0, 3)] private float _time = 0.25f, _cooldown = 1.5f;
        [SerializeField] private UnityEvent<bool> _onMovementChanged;

        private Rigidbody2D _body;
        private WaitUntil _groundCheck;
        private WaitForSeconds _dashTime, _dashCooldown;

        private float _inputDirection, _scale;
        private bool _isGrounded, _canDash = true;

        private void Awake() => _body = GetComponent<Rigidbody2D>();
        private void Start() => _scale = _body.gravityScale;
        private void OnEnable() { _dashTime = new(_time); _dashCooldown = new(_cooldown); _groundCheck = new(() => _isGrounded); }

        private void OnMove(InputValue value) => _inputDirection = value.Get<Vector2>().x;
        private void OnDash() { if (_canDash && _inputDirection != 0) { StopAllCoroutines(); StartCoroutine(DashEffect()); } }
        private void OnCollisionEnter2D(Collision2D collision) => _isGrounded = true;
        private void OnCollisionExit2D(Collision2D collision) => _isGrounded = false;

        private IEnumerator DashEffect()
        {
            _canDash = false;
            _onMovementChanged.Invoke(false);

            _body.gravityScale = 0;
            _body.linearVelocity = new(_inputDirection * _speed, 0f);

            yield return _dashTime;
            _body.gravityScale = _scale;
            _onMovementChanged.Invoke(true);

            yield return _groundCheck;
            yield return _dashCooldown;
            _canDash = true;
        }
    }
}