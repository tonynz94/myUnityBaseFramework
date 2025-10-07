using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        Max
    }

    public enum PopupCommonType
    {
        YES,
        YESNO
    }
}

public enum MESSAGE_EVENT_TYPE
{
    MESS_MAXCOUNT
}


public class MessageSystem
{
    static Action<object>[] _eventProcDelegates = new Action<object>[(int)MESSAGE_EVENT_TYPE.MESS_MAXCOUNT];

    public static void CallEventMessage(MESSAGE_EVENT_TYPE evtType, object obj = null)
    {
        _eventProcDelegates[(int)evtType]?.Invoke(obj);
    }

    public static void RegisterMessageSystem(int evtType, Action<object> evt)
    {
        _eventProcDelegates[evtType] += evt;
    }

    public static void UnRegisterMessageSystem(int evtType, Action<object> evt)
    {
        _eventProcDelegates[evtType] -= evt;
    }
}

