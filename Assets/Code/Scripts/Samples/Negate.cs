using UnityEngine;
using UnityEngine.Events;

public class Negate : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> _onValueChanged;

    public void SetValue(bool value) => _onValueChanged?.Invoke(!value);
}