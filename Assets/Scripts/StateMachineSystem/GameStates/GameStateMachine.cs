using System;
using System.Collections.Generic;
using NetWork;
using UnityEngine;

public class GameStateMachine : StateMachine
{
    [SerializeField] private GameState[] _states;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        stateDic = new Dictionary<Type, IState>(_states.Length);
        foreach (GameState gameState in _states)
        {
            stateDic.Add(gameState.GetType(), gameState);
        }
    }

    private void Start()
    {
        SwitchOn(GetState<GameState_Start>());
    }

    private void OnDestroy()
    {
        NetManager.Instance.SendNoAsync(new QuitMsg());
        NetManager.Instance.Close();
    }
}
