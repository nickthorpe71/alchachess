using UnityEngine;

namespace Logic
{
    public static class Helpers
    {
        public static string GetClassAsStr<T>(T obj) => obj == null ? "null" : obj.GetType().Name;
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
            }
            return null;
        }
    }
}
