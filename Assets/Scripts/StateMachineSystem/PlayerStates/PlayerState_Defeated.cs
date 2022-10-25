using UnityEngine;

// 死亡状态
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Defeated", fileName = "PlayerState_Defeated")]
public sealed class PlayerState_Defeated : PlayerState
{
    [SerializeField] private ParticleSystem defeatedVFX;
    [SerializeField] private AudioClip[] voices;

    public override void Enter()
    {
        base.Enter();

        Instantiate(defeatedVFX, playerController.transform.position, Quaternion.identity);

        AudioClip clip = voices[Random.Range(0, voices.Length)];
        playerController.VoicePlayer.PlayOneShot(clip);
    }

    public override void LogicUpdate()
    {
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(stateMachine.GetState<PlayerState_Float>());
        }
    }
}
