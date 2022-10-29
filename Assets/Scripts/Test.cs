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
            NetManager.Instance.Send("hhhhhh");
        }
    }
}
