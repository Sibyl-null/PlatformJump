using System;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : BasePanel
{
    public Button btnPush;
    public Button btnBack;
    public InputField inputName;
    public Text txtTime;
    
    [SerializeField] private EventCenterVoid comeBackEvent;   // 返回开始界面事件
    private float recordTime = Single.MaxValue;

    private void Start()
    {
        btnPush.onClick.AddListener(() =>
        {
            
        });
        
        btnBack.onClick.AddListener(() =>
        {
            comeBackEvent.EventTrigger();
        });
    }

    public void InitTime(float time)
    {
        recordTime = time;
        
        int minute = (int)(time / 60);
        int second = (int)time % 60;

        txtTime.text = Helper.FormatTime(minute) + ":" + Helper.FormatTime(second);
    }
}
