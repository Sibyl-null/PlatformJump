using NetWork;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    public Button btnStart;
    public Button btnRank;

    [SerializeField] private EventCenterVoid enterGameEvent; 
    
    private void Start()
    {
        btnStart.onClick.AddListener(() =>
        {
            enterGameEvent.EventTrigger();
        });
        
        btnRank.onClick.AddListener(() =>
        {
            GetRankMsg msg = new GetRankMsg();
            NetManager.Instance.Send(msg, () =>
            {
                Debug.Log("get rank");
            });
        });
    }
}
