using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ClearTimerPanel : BasePanel
{
    public Text txtTime;

    [SerializeField] private EventCenterVoid levelStartEvent;
    private float _nowTime = 0;
    private bool _hasCount = false;

    private void OnEnable()
    {
        levelStartEvent.AddEventListener(StartTimeCount);
    }

    private void Update()
    {
        if (_hasCount)
        {
            _nowTime += Time.deltaTime;

            int minute = (int)(_nowTime / 60);
            int second = (int)_nowTime % 60;

            txtTime.text = FormatTime(minute) + ":" + FormatTime(second);
        }
    }

    private void OnDisable()
    {
        _hasCount = false;
        levelStartEvent.RemoveEventListener(StartTimeCount);
    }

    // 开始计时
    private void StartTimeCount()
    {
        _hasCount = true;
    }

    private string FormatTime(int num)
    {
        return num < 10 ? $"0{num}" : num.ToString();
    }
}
