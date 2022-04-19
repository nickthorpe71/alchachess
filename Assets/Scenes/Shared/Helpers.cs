using UnityEngine;

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
    public static Vector3 V2toV3(Vector2 pos, float height) => new Vector3(pos.x, height, pos.y);
}