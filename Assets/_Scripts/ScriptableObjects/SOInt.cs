using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInt", menuName = "Scriptable Objects/SOInt")]
public class SOInt : ScriptableObject
{
    [SerializeField] private int _value;

    public Action<int> OnValueChanged;

    public int Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }
}
