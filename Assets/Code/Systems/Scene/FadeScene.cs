using System;
using UnityEngine.Animations;

namespace UnityEngine.SceneManagement
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeScene : TweenAlpha
    {
        private CanvasGroup _canvasGroup;
        public Action<bool> onCompleted;

        protected override void Awake() { base.Awake(); _canvasGroup = GetComponent<CanvasGroup>(); }
        private void Start()
        {
            _tweenCore.IsEnabled = onCompleted == null;
            _canvasGroup.alpha = _alpha = _tweenCore.IsEnabled ? Value(1f) : Value(0f);
            if (_tweenCore.IsEnabled) FadeOut(); else FadeIn();
        }
        protected override void OnUpdate(float value) { base.OnUpdate(value); _canvasGroup.alpha = value; }
        protected override void OnComplete()
        {
            if (!_tweenCore.IsEnabled) Destroy(gameObject);
            onCompleted?.Invoke(true);
            base.OnComplete();
        }
    }
}