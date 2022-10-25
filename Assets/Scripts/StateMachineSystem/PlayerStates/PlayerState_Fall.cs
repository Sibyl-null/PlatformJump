using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Fall", fileName = "PlayerState_Fall")]
public class PlayerState_Fall : PlayerState
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float moveSpeed = 3f;   //空中移动的速度
    
    public override void LogicUpdate()
    {
        if (playerController.IsGrounded)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Land>());
        }

        if (playerInput.Jump)
        {
            if (playerController.CanAirJump)
                stateMachine.SwitchState(stateMachine.GetState<PlayerState_AirJump>());
            else
                playerInput.SetJumpInputBufferTimer();
        }
    }

    public override void PhysicUpdate()
    {
        playerController.Move(playerController.IsWalled ? 0 : moveSpeed);
        //用动画曲线控制下落速度
        playerController.SetVelocityY(curve.Evaluate(StateDuration));
    }
}
