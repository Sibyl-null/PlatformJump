using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Run", fileName = "PlayerState_Run")]
public class PlayerState_Run : PlayerState
{
    
    [SerializeField] private float runSpeed = 5f;

    [SerializeField] private float acceleration = 5f;    //加速度
    
    public override void Enter()
    {
        base.Enter();
        
        currentSpeed = playerController.MoveSpeed;
    }

    public override void LogicUpdate()
    {
        if (!playerInput.Move)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Idle>());
        }
        
        if (playerInput.Jump)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Jump>());
        }
        
        if (!playerController.IsGrounded)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_CoyoteTime>());
        }
        
        currentSpeed = Mathf.MoveTowards(currentSpeed, runSpeed, acceleration * Time.deltaTime);
    }

    public override void PhysicUpdate()
    {
        playerController.Move(currentSpeed);
    }
}
