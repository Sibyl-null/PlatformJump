using System;
using UnityEngine;

/// <summary>
/// MVVM 可绑定属性
/// </summary>
public class BindableProperty<T> where T : IEquatable<T>
{
    private T _value;
    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            if (_value == null || !_value.Equals(value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }
}
