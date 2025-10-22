using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WhiteArrow
{
    [Serializable]
    public class InterfacesList<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IList
        where T : class
    {
        [SerializeField] private List<InterfaceField<T>> _items = new();



        public int Count => _items.Count;
        public bool IsReadOnly => false;


        bool IList.IsFixedSize => (_items as IList).IsFixedSize;
        bool IList.IsReadOnly => (_items as IList).IsReadOnly;
        int ICollection.Count => _items.Count;
        bool ICollection.IsSynchronized => (_items as ICollection).IsSynchronized;
        object ICollection.SyncRoot => (_items as ICollection).SyncRoot;



        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = (T)value;
        }

        public T this[int index]
        {
            get => _items[index].Value;
            set => _items[index] = value;
        }



        public bool Contains(T item) => _items.Contains(item);
        public int IndexOf(T item) => _items.IndexOf(item);

        public void Add(T item) => _items.Add(item);
        public void AddRange(IEnumerable<T> collection) => _items.AddRange(collection.Select(i => new InterfaceField<T>(i)));
        public void Insert(int index, T item) => _items.Insert(index, item);

        public bool Remove(T item) => _items.Remove(item);
        public void RemoveAt(int index) => _items.RemoveAt(index);
        public void RemoveRange(int index, int count) => _items.RemoveRange(index, count);

        public void Clear() => _items.Clear();



        public void CopyTo(T[] array, int arrayIndex)
        {
            var castedArray = array.Select(e => new InterfaceField<T>(e)).ToArray();
            _items.CopyTo(castedArray, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            var castedArray = array.OfType<InterfaceField<T>>().ToArray();
            _items.CopyTo(castedArray, index);
        }



        public IEnumerator<T> GetEnumerator() => _items.Select(e => e.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



        bool IList.Contains(object value) => (_items as IList).Contains(value);
        int IList.IndexOf(object value) => (_items as IList).IndexOf(value);

        int IList.Add(object value) => (_items as IList).Add(value);
        void IList.Insert(int index, object value) => (_items as IList).Insert(index, value);
        void IList.Remove(object value) => (_items as IList).Remove(value);
        void IList.RemoveAt(int index) => (_items as IList).RemoveAt(index);

        void IList.Clear() => (_items as IList).Clear();

        void ICollection.CopyTo(Array array, int index) => (_items as ICollection).CopyTo(array, index);
    }
}