using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/StateMachine/GameState/Game", fileName = "GameState_Game")]
public class GameState_Game : GameState
{
    [SerializeField] private EventCenterVoid playerWinEvent;   // 玩家成功事件
    [SerializeField] private EventCenterVoid playerDefeatedEvent;   // 玩家失败事件

    [SerializeField] private EventCenterVoid comeBackEvent;   // 返回开始界面事件

    public override void Enter()
    {
        playerWinEvent.AddEventListener(OnPlayerWinEvent);
        comeBackEvent.AddEventListener(OnComeBack);
        
        SceneManager.LoadSceneAsync(GlobalString.GAME_SCENE);

        UIManager.Instance.ShowPanel<ReadyPanel>(GlobalString.READY_PANEL);
        UIManager.Instance.ShowPanel<ClearTimerPanel>(GlobalString.CLEARTIMER_PANEL);
    }

    public override void LogicUpdate()
    { 
        // 作弊按钮，按 F1 通关
        #if UNITY_EDITOR
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            playerWinEvent.EventTrigger();
        }
        #endif
    }

    public override void Exit()
    {
        playerDefeatedEvent.RemoveEventListener(OnPlayerWinEvent);
        comeBackEvent.RemoveEventListener(OnComeBack);
    }

    private void OnPlayerWinEvent()
    {
        VictoryPanel victoryPanel = UIManager.Instance.ShowPanel<VictoryPanel>(GlobalString.VICTORY_PANEL);
        victoryPanel.InitTime(UIManager.Instance.GetPanel<ClearTimerPanel>(GlobalString.CLEARTIMER_PANEL).RecordTime);
        UIManager.Instance.DestroyPanel(GlobalString.CLEARTIMER_PANEL);
    }

    private void OnComeBack()
    {
        UIManager.Instance.DestroyAllPanel();
        stateMachine.SwitchState(stateMachine.GetState<GameState_Start>());
    }
}
