using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle", fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    [SerializeField] private float deceleration = 5f;    //减速度
    
    public override void Enter()
    {
        base.Enter();

        currentSpeed = playerController.MoveSpeed;
    }

    public override void LogicUpdate()
    {
        if (playerInput.Move)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Run>());
        }

        if (playerInput.Jump)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Jump>());
        }

        if (!playerController.IsGrounded)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Fall>());
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
    }

    public override void PhysicUpdate()
    {
        playerController.SetVelocityX(currentSpeed * playerController.transform.localScale.x);
    }
}
