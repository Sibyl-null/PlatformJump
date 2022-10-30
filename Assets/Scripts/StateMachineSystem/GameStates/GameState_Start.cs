using NetWork;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/GameState/Start", fileName = "GameState_Start")]
public class GameState_Start : GameState
{
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;
    
    public override void Enter()
    {
        // 进入游戏尝试网络连接
        NetManager.Instance.Connect(ipAddress, port);
    }
}
