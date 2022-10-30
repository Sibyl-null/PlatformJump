using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatPanel : BasePanel
{
    public Button btnRetry;
    public Button btnBack;
    
    [SerializeField] private EventCenterVoid comeBackEvent;   // 返回开始界面事件

    private void Start()
    {
        btnRetry.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(GlobalString.GAME_SCENE);
            UIManager.Instance.DestroyPanel(GlobalString.DEFEAT_PANEL);
        });
        
        btnBack.onClick.AddListener(() =>
        {
            comeBackEvent.EventTrigger();
        });
    }
}
