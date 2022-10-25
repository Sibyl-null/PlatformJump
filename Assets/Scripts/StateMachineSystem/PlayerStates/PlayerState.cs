using UnityEngine;

public class PlayerState : ScriptableObject, IState
{
    protected Animator animator;
    protected PlayerStateMachine stateMachine;
    protected PlayerInput playerInput;
    protected PlayerController playerController;
    
    protected float currentSpeed;   //当前移动速度

    [SerializeField][Range(0f, 1f)] private float transitionDuration = 0.1f;   //动画切换过渡时间
    [SerializeField] private E_State state;
    private int _stateHash;
    private float _stateStartTime;   //动画刚开始播放的时间

    protected float StateDuration => Time.time - _stateStartTime;   //动画播了多久了
    //当前动画是否播放完成
    protected bool IsAnimationFinished => StateDuration >= animator.GetCurrentAnimatorStateInfo(0).length;

    public void Init(Animator anima, PlayerStateMachine sm, PlayerInput input, PlayerController controller)
    {
        _stateHash = Animator.StringToHash(Helper.GetStateString(state));
        animator = anima;
        stateMachine = sm;
        playerInput = input;
        playerController = controller;
    }
    
    public virtual void Enter()
    {
        animator.CrossFade(_stateHash, transitionDuration);
        _stateStartTime = Time.time;
    }

    public virtual void Exit()
    {
        
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicUpdate()
    {
        
    }

    public override string ToString()
    {
        return Helper.GetStateString(state);
    }
}
