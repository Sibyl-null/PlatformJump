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
            //TODO:
        });
    }
}
