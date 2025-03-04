using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Environment
{
    public enum Polarity { Positive, Negative }

    public class MagnetSwitcher : Singleton<MagnetSwitcher>
    {
        [SerializeField] private InputActionReference _input;
        [SerializeField] private Polarity _currentPolarity;

        public event Action<Polarity> onPolarityChanged;

        protected override void Awake() { base.Awake(); _input.action.performed += InputCallback; }
        private void Start() => SetPolarity(_currentPolarity);
        private void OnEnable() => _input.action.Enable();
        private void OnDisable() => _input.action.Disable();

        private void OnValidate() => SetPolarity(_currentPolarity);
        private void InputCallback(InputAction.CallbackContext ctx) => SwipePolarity();

        private void SetPolarity(Polarity value) => onPolarityChanged?.Invoke(value);
        private void SwipePolarity()
        {
            _currentPolarity = _currentPolarity.Equals(Polarity.Positive) ? Polarity.Negative : Polarity.Positive;
            SetPolarity(_currentPolarity);
        }
    }
}