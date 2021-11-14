using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static GameObject Generate(string prefab)
    {
        return Object.Instantiate(Resources.Load<GameObject>("Prefabs/" + prefab));
    }
    public static GameObject Generate(string prefab,GameObject parent)
    {
        GameObject child = Generate(prefab);
        child.transform.SetParent(parent.transform);
        child.transform.position = parent.transform.position;
        return child;
    }
}
