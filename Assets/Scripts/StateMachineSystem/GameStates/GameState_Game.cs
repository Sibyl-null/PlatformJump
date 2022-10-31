using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/StateMachine/GameState/Game", fileName = "GameState_Game")]
public class GameState_Game : GameState
{
    [SerializeField] private EventCenterVoid playerWinEvent;   // 玩家成功事件
    [SerializeField] private EventCenterVoid playerDefeatedEvent;   // 玩家失败事件

    [SerializeField] private EventCenterVoid comeBackEvent;   // 返回开始界面事件

    public override async void Enter()
    {
        playerWinEvent.AddEventListener(OnPlayerWinEvent);
        playerDefeatedEvent.AddEventListener(OnPlayerDefeatedEvent);
        comeBackEvent.AddEventListener(OnComeBack);
        
        await SceneManager.LoadSceneAsync(GlobalString.GAME_SCENE);

        UIManager.Instance.ShowPanel<ReadyPanel>(GlobalString.READY_PANEL);
        UIManager.Instance.ShowPanel<ClearTimerPanel>(GlobalString.CLEARTIMER_PANEL);
    }

    public override void Exit()
    {
        playerDefeatedEvent.RemoveEventListener(OnPlayerWinEvent);
        playerDefeatedEvent.RemoveEventListener(OnPlayerDefeatedEvent);
        comeBackEvent.RemoveEventListener(OnComeBack);
    }

    private void OnPlayerWinEvent()
    {
        VictoryPanel victoryPanel = UIManager.Instance.ShowPanel<VictoryPanel>(GlobalString.VICTORY_PANEL);
        victoryPanel.InitTime(UIManager.Instance.GetPanel<ClearTimerPanel>(GlobalString.CLEARTIMER_PANEL).RecordTime);
        UIManager.Instance.DestroyPanel(GlobalString.CLEARTIMER_PANEL);
    }

    private void OnPlayerDefeatedEvent()
    {
        UIManager.Instance.DestroyPanel(GlobalString.CLEARTIMER_PANEL);
        UIManager.Instance.ShowPanel<DefeatPanel>(GlobalString.DEFEAT_PANEL);
    }

    private void OnComeBack()
    {
        UIManager.Instance.DestroyAllPanel();
        stateMachine.SwitchState(stateMachine.GetState<GameState_Start>());
    }
}
