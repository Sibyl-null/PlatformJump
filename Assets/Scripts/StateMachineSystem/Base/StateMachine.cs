using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState _currentState;

    protected Dictionary<Type, IState> stateDic;

    protected virtual void Update()
    {
        _currentState.LogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        _currentState.PhysicUpdate();
    }

    /// <summary>
    /// 进入一个状态
    /// </summary>
    public void SwitchOn(IState state)
    {
        _currentState = state;
        _currentState.Enter();
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    public void SwitchState(IState state)
    {
        _currentState.Exit();
        SwitchOn(state);
    }

    /// <summary>
    /// 获取对于状态
    /// </summary>
    public IState GetState<T>()
    {
        Type type = typeof(T);
        if (stateDic.ContainsKey(type))
        {
            return stateDic[type];
        }

        return default;
    }
}
