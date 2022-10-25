using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] private PlayerState[] _states;
    
    private Animator _animator;
    private PlayerInput _playerInput;
    private PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerController = GetComponent<PlayerController>();

        stateDic = new Dictionary<Type, IState>(_states.Length);
        foreach (PlayerState playerState in _states)
        {
            playerState.Init(_animator, this, _playerInput, _playerController);
            stateDic.Add(playerState.GetType(), playerState);
        }
    }

    private void Start()
    {
        SwitchOn(GetState<PlayerState_Idle>());
    }
}
