using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
    [RequireComponent(typeof(PlayerCore))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _accelRate = 25;

        private PlayerCore _player;
        private int _input;

        private void Awake() => _player = GetComponent<PlayerCore>();
        private void OnMove(InputValue value) => _input = (int)value.Get<Vector2>().x;

        private void FixedUpdate()
        {
            //calculo de movimiento
            float targetSpeed = _input * _speed;
            float speedDif = targetSpeed - _player.HorizontalVelocity;
            float movement = speedDif * _accelRate;

            //cambio de direccion
            if (_input != 0) transform.localScale = new Vector3(_input, 1, 1);

            //aplicacion de fuerza
            if (movement != 0) _player.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
    }
}