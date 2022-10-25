using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EventCenterVoid", fileName = "EventCenterVoid_")]
public class EventCenterVoid : ScriptableObject
{
    private Action action;

    public void AddEventListener(Action ac)
    {
        action += ac;
    }

    public void RemoveEventListener(Action ac)
    {
        action -= ac;
    }

    public void EventTrigger()
    {
        action?.Invoke();
    }
}
