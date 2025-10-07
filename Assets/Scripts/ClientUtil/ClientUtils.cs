using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }
    public static T FindChild<T>(GameObject go, string name = null, bool recusive = true) where T : UnityEngine.Object
    {
        if (recusive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (transform.name == name || string.IsNullOrEmpty(name))
                {
                    return transform.GetComponent<T>();
                }
            }
        }
        else
        {
            foreach (var co in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || co.name == name)
                {
                    return co;
                }
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = true)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;

        return null;
    }
}
