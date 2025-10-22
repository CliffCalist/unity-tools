using UnityEngine;

namespace WhiteArrow
{
    public abstract class ComponentProvider<T> : MonoBehaviour
    {
        private T _componentCached;


        public T Component => _componentCached ??= GetComponent<T>();
    }
}