using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Win", fileName = "PlayerState_Win")]
public class PlayerState_Win : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        
        playerInput.DisableGamePlayerInputs();
    }
}
