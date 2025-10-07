using System;
using System.Collections.Generic;

public delegate void MessengerCallback();

public delegate void MessengerCallback<T>(T arg1);

public delegate void MessengerCallback<T, U>(T arg1, U arg2);

public delegate void MessengerCallback<T, U, V>(T arg1, U arg2, V arg3);

public delegate void MessengerCallback<T, U, V, W>(T arg1, U arg2, V arg3, W arg4);

public delegate void MessengerCallback<T, U, V, W, X>(T arg1, U arg2, V arg3, W arg4, X arg5);

public enum MessengerMode : byte
{
    DONT_REQUIRE_LISTENER,
    REQUIRE_LISTENER,
}

// HACK: bool을 안 쓰고, 아래 enum을 만든 이유는 Listener 등록/해제 코드들 검색을 쉽게 하기 위함이다.
public enum MessengerLRT
{
    ADD_LISTENER,
    REMOVE_LISTENER
}

public enum MsgID
{

}

internal static class EventMessengerInternal
{
    public static Dictionary<MsgID, List<Delegate>> EventDic = new Dictionary<MsgID, List<Delegate>>();
    public static readonly MessengerMode DEFAULT_MODE = MessengerMode.DONT_REQUIRE_LISTENER;

    public static void AddListener(MsgID msgID, Delegate listener)
    {
        if (listener == null)
        {
            return;
        }

        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            if (delegateList.Contains(listener))
            {
                return;
            }

            if (delegateList.Count > 0)
            {
                Delegate d = delegateList[0];

                if (d != null && d.GetType() != listener.GetType())
                {
                    throw new Exception(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}",
                        msgID, d.GetType().Name, listener.GetType().Name));
                }
            }

            delegateList.Add(listener);
        }
        else
        {
            delegateList = new List<Delegate>();
            delegateList.Add(listener);

            EventDic.Add(msgID, delegateList);
        }
    }

    public static void RemoveListener(MsgID msgID, Delegate listener)
    {
        if (listener == null)
        {
            return;
        }

        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            delegateList.Remove(listener);

            if (delegateList.Count <= 0)
            {
                EventDic.Remove(msgID);
            }
        }
    }
}

//
// No parameters
//
public static class EventMessenger
{
    private static Dictionary<MsgID, List<Delegate>> EventDic = EventMessengerInternal.EventDic;

    public static void RegisterListener(MessengerLRT val, MsgID msgID, MessengerCallback handler)
    {
        if (val == MessengerLRT.ADD_LISTENER)
        {
            EventMessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MessengerLRT.REMOVE_LISTENER)
        {
            EventMessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID)
    {
        Broadcast(msgID, EventMessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, MessengerMode mode)
    {
        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                MessengerCallback callback = delegateList[i] as MessengerCallback;
                if (callback != null)
                {
                    if (callback.Target == null)
                        delegateList.RemoveAt(i--);
                    else
                        callback();
                }
                else
                {
                    throw new Exception(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", msgID));
                }
            }
        }
        else
        {
            if (mode == MessengerMode.REQUIRE_LISTENER)
            {
                throw new Exception(string.Format("Broadcasting message {0} but no listener found.", msgID));
            }
        }
    }
}


//
// One parameters
//
public static class EventMessenger<T>
{
    private static Dictionary<MsgID, List<Delegate>> EventDic = EventMessengerInternal.EventDic;

    public static void RegisterListener(MessengerLRT val, MsgID msgID, MessengerCallback<T> handler)
    {
        if (val == MessengerLRT.ADD_LISTENER)
        {
            EventMessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MessengerLRT.REMOVE_LISTENER)
        {
            EventMessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1)
    {
        Broadcast(msgID, arg1, EventMessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, MessengerMode mode)
    {
        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                MessengerCallback<T> callback = delegateList[i] as MessengerCallback<T>;
                if (callback != null)
                {
                    callback(arg1);
                }
                else
                {
                    throw new Exception(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", msgID));
                }
            }
        }
        else
        {
            if (mode == MessengerMode.REQUIRE_LISTENER)
            {
                throw new Exception(string.Format("Broadcasting message {0} but no listener found.", msgID));
            }
        }
    }
}


//
// Two parameters
//
public static class EventMessenger<T, U>
{
    private static Dictionary<MsgID, List<Delegate>> EventDic = EventMessengerInternal.EventDic;

    public static void RegisterListener(MessengerLRT val, MsgID msgID, MessengerCallback<T, U> handler)
    {
        if (val == MessengerLRT.ADD_LISTENER)
        {
            EventMessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MessengerLRT.REMOVE_LISTENER)
        {
            EventMessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2)
    {
        Broadcast(msgID, arg1, arg2, EventMessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, MessengerMode mode)
    {
        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                MessengerCallback<T, U> callback = delegateList[i] as MessengerCallback<T, U>;
                if (callback != null)
                {
                    callback(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", msgID));
                }
            }
        }
        else
        {
            if (mode == MessengerMode.REQUIRE_LISTENER)
            {
                throw new Exception(string.Format("Broadcasting message {0} but no listener found.", msgID));
            }
        }
    }
}


//
// Three parameters
//
public static class EventMessenger<T, U, V>
{
    private static Dictionary<MsgID, List<Delegate>> EventDic = EventMessengerInternal.EventDic;

    public static void RegisterListener(MessengerLRT val, MsgID msgID, MessengerCallback<T, U, V> handler)
    {
        if (val == MessengerLRT.ADD_LISTENER)
        {
            EventMessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MessengerLRT.REMOVE_LISTENER)
        {
            EventMessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3)
    {
        Broadcast(msgID, arg1, arg2, arg3, EventMessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, MessengerMode mode)
    {
        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                MessengerCallback<T, U, V> callback = delegateList[i] as MessengerCallback<T, U, V>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", msgID));
                }
            }
        }
        else
        {
            if (mode == MessengerMode.REQUIRE_LISTENER)
            {
                throw new Exception(string.Format("Broadcasting message {0} but no listener found.", msgID));
            }
        }
    }
}


//
// Four parameters
//
public static class EventMessenger<T, U, V, W>
{
    private static Dictionary<MsgID, List<Delegate>> EventDic = EventMessengerInternal.EventDic;

    public static void RegisterListener(MessengerLRT val, MsgID msgID, MessengerCallback<T, U, V, W> handler)
    {
        if (val == MessengerLRT.ADD_LISTENER)
        {
            EventMessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MessengerLRT.REMOVE_LISTENER)
        {
            EventMessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4)
    {
        Broadcast(msgID, arg1, arg2, arg3, arg4, EventMessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, MessengerMode mode)
    {
        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                MessengerCallback<T, U, V, W> callback = delegateList[i] as MessengerCallback<T, U, V, W>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", msgID));
                }
            }
        }
        else
        {
            if (mode == MessengerMode.REQUIRE_LISTENER)
            {
                throw new Exception(string.Format("Broadcasting message {0} but no listener found.", msgID));
            }
        }
    }
}


//
// Five parameters
//
public static class EventMessenger<T, U, V, W, X>
{
    private static Dictionary<MsgID, List<Delegate>> EventDic = EventMessengerInternal.EventDic;


    public static void RegisterListener(MessengerLRT val, MsgID msgID, MessengerCallback<T, U, V, W, X> handler)
    {
        if (val == MessengerLRT.ADD_LISTENER)
        {
            EventMessengerInternal.AddListener(msgID, handler);
        }
        else if (val == MessengerLRT.REMOVE_LISTENER)
        {
            EventMessengerInternal.RemoveListener(msgID, handler);
        }
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, X arg5)
    {
        Broadcast(msgID, arg1, arg2, arg3, arg4, arg5, EventMessengerInternal.DEFAULT_MODE);
    }

    public static void Broadcast(MsgID msgID, T arg1, U arg2, V arg3, W arg4, X arg5, MessengerMode mode)
    {
        List<Delegate> delegateList;
        if (EventDic.TryGetValue(msgID, out delegateList))
        {
            for (int i = 0; i < delegateList.Count; ++i)
            {
                MessengerCallback<T, U, V, W, X> callback = delegateList[i] as MessengerCallback<T, U, V, W, X>;
                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", msgID));
                }
            }
        }
        else
        {
            if (mode == MessengerMode.REQUIRE_LISTENER)
            {
                throw new Exception(string.Format("Broadcasting message {0} but no listener found.", msgID));
            }
        }
    }
}

public interface iEventMessanger
{
    public void Register(MessengerLRT type);
}