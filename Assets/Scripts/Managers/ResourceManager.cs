using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public void Init()
    {
    }

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefeb = Load<GameObject>($"Prefabs/{path}");
        if (prefeb == null)
            return null;

        if (prefeb.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(prefeb, parent).gameObject;

        return Instantiate(prefeb, parent);
    }

    public GameObject Instantiate(GameObject prefeb, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefeb, parent);
        go.name = prefeb.name;
        return go;
    }

    public void Destroy(GameObject go, float sec = 0)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();

        //만약에 풀어블을 가지고 있다면
        if (poolable != null)
        {
            //오브젝트를 파괴시키지 않고 다시 풀에다가 반환을 하는 것.
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go, sec);
    }
}
