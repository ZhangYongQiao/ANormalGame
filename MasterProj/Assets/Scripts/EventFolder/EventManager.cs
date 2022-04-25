using System;
using System.Collections.Generic;
using UnityEngine;



public class EventManager
{
    private static Dictionary<string,Action<EventParam>> _EventDic = new Dictionary<string, Action<EventParam>>();

    public static void AddListener(string name,Action<EventParam> act)
    {
        if (_EventDic.ContainsKey(name))
        {
            _EventDic[name] += act;
        }
        else
        {
            _EventDic.Add(name, act);
        }
    }

    public static void RemoveListener(string name, Action<EventParam> act)
    {
        if (_EventDic.ContainsKey(name))
        {
            if(_EventDic[name] != null)
            {
                _EventDic[name] -= act;
            }
        }
    }

    public static void Send(string name,EventParam param)
    {
        if (_EventDic.ContainsKey(name))
        {
            _EventDic[name]?.Invoke(param);
        }
        else
        {
            Debug.LogError($"不存在该键  {name}");
        }
    }

}

public class EventName
{
}


public class EventParam
{
    public System.Object obj;

    public EventParam(System.Object obj)
    {
        this.obj = obj;
    }

}
