using UnityEngine;

public class ReadyPanel : BasePanel
{
    [SerializeField] private EventCenterVoid eventCenterStartGate;

    public void LevelStart()
    {
        eventCenterStartGate.EventTrigger();
        
        UIManager.Instance.DestroyPanel(GlobalString.READY_PANEL);
    }
}
