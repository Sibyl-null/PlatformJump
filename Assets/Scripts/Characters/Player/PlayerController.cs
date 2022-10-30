using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    [SerializeField] private PlayerGroundedDetector groundedDetector;   //检测地面
    [SerializeField] private PlayerGroundedDetector walledDetector;     //检测墙面

    [SerializeField] private EventCenterVoid playerWinEvent;

    public float MoveSpeed => Mathf.Abs(_rigidbody.velocity.x);    //当前移动速度
    public bool IsGrounded => groundedDetector.IsGrounded;   //是否在地面上
    public bool IsFalling => _rigidbody.velocity.y < 0f && !IsGrounded;  //是否处于掉落状态
    public bool IsWalled => walledDetector.IsGrounded;   //是否和墙壁接触
    public bool CanAirJump { set; get; }   //是否可以空中跳跃
    
    public AudioSource VoicePlayer { private set; get; }   //玩家语音播放
    public bool Victory { get; private set; } = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        VoicePlayer = GetComponentInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        playerWinEvent.AddEventListener(LevelWin);
    }

    private void OnDisable()
    {
        playerWinEvent.RemoveEventListener(LevelWin);
    }

    private void Start()
    {
        _playerInput.EnableGamePlayerInputs();
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    public void SetVelocityX(float velocityX)
    {
        _rigidbody.velocity = new Vector3(velocityX, _rigidbody.velocity.y, 0);
    }

    public void SetVelocityY(float velocityY)
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, velocityY, 0);
    }

    public void Move(float speed)
    {
        if (_playerInput.Move)
        {
            this.transform.localScale = new Vector3(_playerInput.AxisX, 1f, 1f);
        }
        SetVelocityX(speed * _playerInput.AxisX);
    }

    public void SetUseGravity(bool flag)
    {
        _rigidbody.useGravity = flag;
    }
    
    private void LevelWin()
    {
        Victory = true;
        
        PlayerStateMachine sm = GetComponent<PlayerStateMachine>();
        sm.SwitchState(sm.GetState<PlayerState_Win>());
    }

    public void OnDefeated()
    {
        _playerInput.DisableGamePlayerInputs();
        
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = false;
        _rigidbody.detectCollisions = false;

        PlayerStateMachine sm = GetComponent<PlayerStateMachine>();
        sm.SwitchState(sm.GetState<PlayerState_Defeated>());
    }
}
