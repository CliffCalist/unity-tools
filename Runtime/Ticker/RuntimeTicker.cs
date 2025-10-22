using System;
using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow
{
    public class RuntimeTicker : MonoBehaviour
    {
        private static readonly List<ITickable> s_tickables = new();
        private static RuntimeTicker s_instanceCached;



        private static void CreateInstanceIfNotExists()
        {
            if (s_instanceCached != null)
                return;

            var go = new GameObject("[WA-RuntimeTicker]");
            DontDestroyOnLoad(go);
            s_instanceCached = go.AddComponent<RuntimeTicker>();
        }



        public static void Register(ITickable tickable)
        {
            if (UnityCheck.IsDestroyed(tickable))
                throw new ArgumentNullException(nameof(tickable), "Tickable cannot be null or destroyed.");

            CreateInstanceIfNotExists();

            if (!s_tickables.Contains(tickable))
                s_tickables.Add(tickable);
        }

        public static void Unregister(ITickable tickable)
        {
            s_tickables.Remove(tickable);
        }



        private void Update()
        {
            var delta = Time.deltaTime;
            for (int i = 0; i < s_tickables.Count; i++)
            {
                var tickable = s_tickables[i];

                if (UnityCheck.IsDestroyed(tickable))
                {
                    s_tickables.RemoveAt(i);
                    Debug.LogWarning($"Removed destroyed tickable: {tickable}", this);
                    i--;
                    continue;
                }

                s_tickables[i].Tick(delta);
            }
        }
    }
}