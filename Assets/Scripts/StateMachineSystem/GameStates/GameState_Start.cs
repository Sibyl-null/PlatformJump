using Cysharp.Threading.Tasks;
using NetWork;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/StateMachine/GameState/Start", fileName = "GameState_Start")]
public class GameState_Start : GameState
{
    [SerializeField] private string ipAddress;
    [SerializeField] private int port;

    [SerializeField] private EventCenterVoid enterGameEvent;

    public override async void Enter()
    {
        enterGameEvent.AddEventListener(EnterGameEvent);

        // 如果不在开始场景就切场景
        Scene startScene = SceneManager.GetSceneByName(GlobalString.START_SCENE);
        if (startScene != SceneManager.GetActiveScene())
        {
            await SceneManager.LoadSceneAsync(GlobalString.START_SCENE);
        }

        UIManager.Instance.ShowPanel<DebugPanel>(GlobalString.DEBUG_PANEL);
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
