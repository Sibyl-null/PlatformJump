using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Jump", fileName = "PlayerState_Jump")]
public class PlayerState_Jump : PlayerState
{
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private float moveSpeed = 3f;   //空中移动的速度
    [SerializeField] private ParticleSystem jumpVFX;
    [SerializeField] private AudioClip jumpSFX;

    public override void Enter()
    {
        base.Enter();

        playerInput.HasJumpInputBuffer = false;
        playerController.SetVelocityY(jumpSpeed);
        playerController.VoicePlayer.PlayOneShot(jumpSFX);
        Instantiate(jumpVFX, playerController.transform.position, Quaternion.identity);
    }

    public override void LogicUpdate()
    {
        if (playerInput.StopJump || playerController.IsFalling)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Fall>());
        }
    }

    public override void PhysicUpdate()
    {
        playerController.Move(playerController.IsWalled ? 0 : moveSpeed);
    }
}
