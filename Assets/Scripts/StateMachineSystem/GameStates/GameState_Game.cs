using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/StateMachine/GameState/Game", fileName = "GameState_Game")]
public class GameState_Game : GameState
{
    public override void Enter()
    {
        SceneManager.LoadScene("Test Ground Scene");
    }
}
