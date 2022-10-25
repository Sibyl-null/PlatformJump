using System;
using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    [SerializeField] private EventCenterVoid eventCenterStartGate;

    public void LevelStart()
    {
        eventCenterStartGate.EventTrigger();
        GetComponent<Canvas>().enabled = false;
        GetComponent<Animator>().enabled = false;
    }
}
