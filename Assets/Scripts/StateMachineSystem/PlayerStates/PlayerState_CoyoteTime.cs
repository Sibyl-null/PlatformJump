using UnityEngine;

// 土狼时间状态
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/CoyoteTime", fileName = "PlayerState_CoyoteTime")]
public class PlayerState_CoyoteTime : PlayerState
{
    [SerializeField] private float runSpeed = 5f;

    [SerializeField] private float coyoteTime = 0.1f;
    
    public override void Enter()
    {
        base.Enter();
        
        playerController.SetUseGravity(false);
    }

    public override void Exit()
    {
        playerController.SetUseGravity(true);
    }

    public override void LogicUpdate()
    {
        if (playerInput.Jump)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Jump>());
        }

        // 时间过了，或松开移动键时
        if (StateDuration > coyoteTime || !playerInput.Move)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Fall>());
        }
    }

    public override void PhysicUpdate()
    {
        playerController.Move(runSpeed);
    }
}
