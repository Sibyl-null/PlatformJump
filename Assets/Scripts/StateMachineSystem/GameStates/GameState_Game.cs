using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/StateMachine/GameState/Game", fileName = "GameState_Game")]
public class GameState_Game : GameState
{
    [SerializeField] private EventCenterVoid playerWinEvent;   // 玩家成功事件
    [SerializeField] private EventCenterVoid playerDefeatedEvent;   // 玩家失败事件

    public override void Enter()
    {
        playerWinEvent.AddEventListener(OnPlayerWinEvent);
        
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
    }

    private void OnPlayerWinEvent()
    {
        UIManager.Instance.DestroyPanel(GlobalString.CLEARTIMER_PANEL);
        UIManager.Instance.ShowPanel<VictoryPanel>(GlobalString.VICTORY_PANEL);
    }
}
