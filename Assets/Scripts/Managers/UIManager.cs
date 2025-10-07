using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    int _order = -20;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    public UI_Scene SceneUI { get; private set; }
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public void SetCanvasInWorldSpace(GameObject go)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
    }

    public T ShowSceneUI<T>(string name = null, Transform parent = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>();
        SceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    public T ShowPopupUI<T>(string name = null, Transform parent = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject prefab = Managers.Resource.Load<GameObject>($"Prefabs/UI/Popup/{name}");

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = go.GetOrAddComponent<T>();
        _popupStack.Push(popup);

        if (parent != null)
            go.transform.SetParent(parent);
        else if (SceneUI != null)
            go.transform.SetParent(SceneUI.transform);
        else
            go.transform.SetParent(Root.transform);

        go.transform.localScale = Vector3.one;
        go.transform.localPosition = prefab.transform.position;

        return popup;
    }

    //스택에서 찾아주는거
    public T FindPopup<T>() where T : UI_Popup
    {
        return _popupStack.Where(x => x.GetType() == typeof(T)).FirstOrDefault() as T;
    }

    public T PeekPopupUI<T>() where T : UI_Popup
    {
        if (_popupStack.Count == 0)
            return null;

        return _popupStack.Peek() as T;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        SceneUI = null;
    }

}
