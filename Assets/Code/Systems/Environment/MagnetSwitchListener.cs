using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class MagnetSwitchListener : MonoBehaviour
    {
        [SerializeField] private Polarity _comparePolarity;
        [SerializeField] private UnityEvent<bool> _onStatusChanged;

        private MagnetSwitcher _controller;

        private void Awake() => _controller = MagnetSwitcher.Instance;
        private void OnEnable() => _controller.onPolarityChanged += OnPerformePolarity;
        private void OnDisable() => _controller.onPolarityChanged -= OnPerformePolarity;

        private void OnPerformePolarity(Polarity value) => _onStatusChanged.Invoke(_comparePolarity.Equals(value));
    }
}