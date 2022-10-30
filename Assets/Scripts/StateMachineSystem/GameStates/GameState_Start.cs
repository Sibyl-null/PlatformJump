using System;
using NetWork;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/GameState/Start", fileName = "GameState_Start")]
public class GameState_Start : GameState
{
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;

    [SerializeField] private EventCenterVoid enterGameEvent;

    public override void Enter()
    {
        enterGameEvent.AddEventListener(EnterGameEvent);
        
        UIManager.Instance.ShowPanel<StartPanel>(GlobalString.START_PANEL);
        // 进入游戏尝试网络连接
        NetManager.Instance.Connect(ipAddress, port);
    }

    public override void Exit()
    {
        enterGameEvent.RemoveEventListener(EnterGameEvent);
        
        UIManager.Instance.DestroyPanel(GlobalString.START_PANEL);
    }

    private void EnterGameEvent()
    {
        stateMachine.SwitchState(stateMachine.GetState<GameState_Game>());
    }
}
