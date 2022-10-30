using System;
using NetWork;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Start()
    {
        NetManager.Instance.Connect("127.0.0.1", 8080);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddRecordMsg msg = new AddRecordMsg();
            msg.recordData.name = "acac";
            msg.recordData.record = 5;
            NetManager.Instance.Send(msg);
        }
    }
}
