using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.SceneManagement
{
    public class SceneController : SingletonBasic<SceneController>
    {
        [SerializeField] private RectTransform _parent;
        [SerializeField] private FadeScene _fade;
        [SerializeField] private bool _isBackupScene;

        protected List<string> _scenes = new();
        private static string _lastScene;
        private bool _lock;

        public void SwipeScene(string value) => OnFadeScene(value);
        public void ReturnScene(string value) => OnFadeScene(string.IsNullOrEmpty(_lastScene) ? value : _lastScene);
        public void Quit() => OnFadeScene(string.Empty);

        public IEnumerator AddScene(string value)
        {
            if (_scenes.Contains(value)) yield break;
            yield return SceneManager.LoadSceneAsync(value, LoadSceneMode.Additive);
            _scenes.Add(value);
        }
        public void RemoveScene(string value)
        {
            if (!_scenes.Contains(value)) return;
            SceneManager.UnloadSceneAsync(value, UnloadSceneOptions.None);
            _scenes.Remove(value);
        }
        
        public void OnCutScene(string value)
        {
            _lastScene = _isBackupScene ? SceneManager.GetActiveScene().path : null;
            SceneManager.LoadSceneAsync(value, LoadSceneMode.Single);
        }
        private void OnFadeScene(string value)
        {
            if (_lock || value == SceneManager.GetActiveScene().path) return;

            Instantiate(_fade, _parent).onCompleted += onComplete;
            _lock = true;

            void onComplete(bool _)
            {
                Time.timeScale = 1;
                if (string.IsNullOrEmpty(value)) Application.Quit();
                else OnCutScene(value);
            }
        }
    }
}