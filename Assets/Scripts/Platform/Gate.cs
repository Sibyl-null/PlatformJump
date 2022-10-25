using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private EventCenterVoid eventCenter;

    private void OnEnable()
    {
        eventCenter.AddEventListener(Open);
    }

    private void OnDisable()
    {
        eventCenter.RemoveEventListener(Open);
    }

    private void Open()  //开门
    {
        Destroy(gameObject);
    }
}
