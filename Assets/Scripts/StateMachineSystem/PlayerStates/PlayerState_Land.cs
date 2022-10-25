using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Land", fileName = "PlayerState_Land")]
public class PlayerState_Land : PlayerState
{
    [SerializeField] private float stiffTime = 0.2f;  //硬直时间
    
    public override void Enter()
    {
        base.Enter();
        
        playerController.SetVelocity(Vector3.zero);
    }

    public override void LogicUpdate()
    {
        if (playerInput.HasJumpInputBuffer || playerInput.Jump)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Jump>());
        }

        if (StateDuration < stiffTime) return;   //硬直时间

        if (playerInput.Move)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Run>());
        }

        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Idle>());
        }
    }
}
