using UnityEngine;

namespace WhiteArrow
{
    public static class UnityCheck
    {
        /// <summary>
        /// Performs a safe null check that correctly handles UnityEngine.Object instances,
        /// including cases where the object has been destroyed but the reference is still non-null due to Unity's override.
        /// </summary>
        public static bool IsDestroyed<T>(T obj) where T : class
        {
            if (obj == null)
                return true;

            if (obj is Object unityObj)
                return unityObj == null;

            return false;
        }
    }
}