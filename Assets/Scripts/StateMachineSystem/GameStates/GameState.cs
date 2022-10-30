using UnityEngine;

public class GameState : ScriptableObject, IState
{
    protected GameStateMachine stateMachine;

    public void Init(GameStateMachine sm)
    {
        stateMachine = sm;
    }
    
    public virtual void Enter()
    {
        
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
}
