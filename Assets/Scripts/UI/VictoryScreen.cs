using System;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private EventCenterVoid eventCenterWin;

    private void OnEnable()
    {
        eventCenterWin.AddEventListener(Win);
    }

    private void OnDisable()
    {
        eventCenterWin.RemoveEventListener(Win);
    }

    private void Win()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<Animator>().enabled = true;
    }
}
