using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnBeginDragHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public Action<PointerEventData> OnEndDragHandler = null;
    public void OnBeginDrag(PointerEventData evt)
    {
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(evt);
    }

    public void OnDrag(PointerEventData evt)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(evt);
    }

    public void OnEndDrag(PointerEventData evt)
    {
        if (OnEndDragHandler != null)
            OnEndDragHandler.Invoke(evt);
    }

    public void OnPointerClick(PointerEventData evt)
    {
        //Managers.Sound.Play(Define.Sound.Effect, "Sound_MainButton", volume: 3.0f);
        if (OnClickHandler != null)
            OnClickHandler.Invoke(evt);
    }

    public void OnPointerDown(PointerEventData evt)
    {
        //throw new NotImplementedException();
    }


}
