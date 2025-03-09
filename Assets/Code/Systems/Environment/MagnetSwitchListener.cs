using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class MagnetSwitchListener : MonoBehaviour
    {
        [SerializeField] private Polarity _comparePolarity;
        [SerializeField] private LayerMask _hideLayer;
        [SerializeField] private UnityEvent<bool> _onStatusChanged;

        private MagnetSwitcher _controller;
        private LayerMask _defaultLayer;

        private void Awake() { _controller = MagnetSwitcher.Instance; _defaultLayer = gameObject.layer; }
        private void OnEnable() => _controller.onPolarityChanged += OnPerformePolarity;
        private void OnDisable() => _controller.onPolarityChanged -= OnPerformePolarity;

        private void OnPerformePolarity(Polarity value)
        {
            bool isMagnetized = _comparePolarity.Equals(value);
            gameObject.layer = isMagnetized ? _defaultLayer : _hideLayer;
            _onStatusChanged.Invoke(isMagnetized);
        }
    }
}