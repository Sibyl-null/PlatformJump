using System;
using Cysharp.Threading.Tasks;
using NetWork;
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
    private bool _hasPush = false;

    private void Start()
    {
        PushFinishAsync();
        
        btnPush.onClick.AddListener(() =>
        {
            if (inputName.text.Length >= 3)
            {
                AddRecordMsg msg = new AddRecordMsg
                {
                    recordData = { name = inputName.text, record = (int)recordTime }
                };
                NetManager.Instance.Send(msg, () =>
                {
                    // 此时所处线程非主线程，再线程池中
                    _hasPush = true;
                });
            }
            else
            {
                Debug.Log("名字不要小于三个字");   
            }
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

    private async void PushFinishAsync()
    {
        while (true)
        {
            await UniTask.Yield();
            if (_hasPush)
            {
                Debug.Log("成绩上传成功");
                btnPush.gameObject.SetActive(false);
                inputName.gameObject.SetActive(false);
                break;
            }
        }
    }
}
