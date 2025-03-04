using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] private InputActionReference _input;
    [SerializeField] private TweenCore _animation;

    private bool _isPaused;

    private void Start() => _input.action.performed += InputCallback;
    private void OnEnable() => _input.action.Enable();
    private void OnDisable() => _input.action.Disable();

    private void InputCallback(InputAction.CallbackContext _) => SwipePause();

    public void SwipePause()
    {
        _isPaused = !_isPaused;
        _animation?.Play(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
    }
}