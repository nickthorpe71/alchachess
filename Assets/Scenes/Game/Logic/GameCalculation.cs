using UnityEngine;
using System;

public static class GameCalculation
{
    public static Func<GameObject, Vector3, Quaternion, GameObject> Instantiate;

    public static void Init(Func<GameObject, Vector3, Quaternion, GameObject> instantiate)
    {
        Instantiate = instantiate;
    }

    public static GameObject Spawn(string objPath, Vector3 pos, Quaternion rot)
    {
        return Instantiate(Resources.Load(objPath) as GameObject, pos, rot);
    }
}
