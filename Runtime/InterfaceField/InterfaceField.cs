using System;
using UnityEngine;

namespace WhiteArrow
{
    [Serializable]
    public class InterfaceField<T> where T : class
    {
        [SerializeField] private UnityEngine.Object _unityValue;
        [SerializeReference, InlineProperty] private T _simpleValue;



        public bool HasValue => Value != null;

        public bool IsUnityValueValid => _unityValue is T;
        public UnityEngine.Object RawUnityValue => _unityValue;



        public T Value
        {
            get
            {
                if (_unityValue != null)
                {
                    if (!IsUnityValueValid)
                        throw new InvalidOperationException($"{nameof(InterfaceField<T>)} is not valid");

                    return _unityValue as T;
                }

                return _simpleValue;
            }
            set
            {
                if (value == null)
                {
                    _unityValue = null;
                    _simpleValue = default;
                }
                else if (value is UnityEngine.Object)
                {
                    _unityValue = value as UnityEngine.Object;
                    _simpleValue = default;
                }
                else
                {
                    _unityValue = null;
                    _simpleValue = value;
                }
            }
        }



        public InterfaceField() { }

        public InterfaceField(T value)
        {
            Value = value;
        }

        public void Clear()
        {
            _unityValue = null;
            _simpleValue = default;
        }



        public override string ToString() => Value?.ToString() ?? "null";

        public override bool Equals(object obj)
        {
            if (obj is InterfaceField<T> other)
                return Equals(Value, other.Value);

            if (obj is T tObj)
                return Equals(Value, tObj);

            return false;
        }

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;


        public static implicit operator T(InterfaceField<T> field) => field.Value;
        public static implicit operator InterfaceField<T>(T value) => new InterfaceField<T>(value);

        public static bool operator ==(InterfaceField<T> a, T b) => Equals(a?.Value, b);
        public static bool operator !=(InterfaceField<T> a, T b) => !Equals(a?.Value, b);
        public static bool operator ==(T a, InterfaceField<T> b) => Equals(a, b?.Value);
        public static bool operator !=(T a, InterfaceField<T> b) => !Equals(a, b?.Value);
    }
}