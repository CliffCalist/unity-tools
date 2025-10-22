using System.Collections;
using UnityEngine;

namespace WhiteArrow
{
    public class Coroutines : MonoBehaviour
    {
        private static SingltonRef s_instanceRef = new();



        public static Coroutine Launch(IEnumerator routine)
        {
            return s_instanceRef.Value.StartCoroutine(routine);
        }



        private class SingltonRef
        {
            private Coroutines _ref;

            public Coroutines Value
            {
                get
                {
                    if (_ref == null)
                    {
                        _ref = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
                        DontDestroyOnLoad(_ref);
                    }
                    return _ref;
                }
            }
        }
    }
}