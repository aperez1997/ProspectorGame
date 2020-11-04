using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    // courtesy https://forum.unity.com/threads/transform-find-doesnt-work.12949/
    public static GameObject FindInChildren(GameObject gameObject, string name)
    {
        foreach (var x in gameObject.GetComponentsInChildren<Transform>())
            if (x.gameObject.name == name)
                return x.gameObject;
        throw new System.Exception("Technically the old version throws an exception if none are found, so I'll do the same here!");
    }
}
