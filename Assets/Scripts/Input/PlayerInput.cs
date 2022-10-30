using System;
using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputAction _playerInputAction;

    [SerializeField] private float jumpInputBufferTime = 0.5f;   // 跳跃缓冲有效时间
    private WaitForSeconds _waitJumpInputBuffer;

    private Vector2 Axes => _playerInputAction.Gameplay.Axes.ReadValue<Vector2>();

    public bool Jump => _playerInputAction.Gameplay.Jump.WasPressedThisFrame();
    public bool StopJump => _playerInputAction.Gameplay.Jump.WasReleasedThisFrame();
    public float AxisX => Axes.x;    //x轴方向的速度
    public bool Move => AxisX != 0f;   //是否处于移动状态
    public bool HasJumpInputBuffer { get; set; }   //是否存在输入缓冲

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();

        _waitJumpInputBuffer = new WaitForSeconds(jumpInputBufferTime);
    }

    private void OnEnable()
    {
        // 提前松开不算
        _playerInputAction.Gameplay.Jump.canceled += (context) =>
        {
            HasJumpInputBuffer = false;
        };
    }

    public void EnableGamePlayerInputs()
    {
        _playerInputAction.Gameplay.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableGamePlayerInputs()
    {
        _playerInputAction.Gameplay.Disable();
    }

    public void SetJumpInputBufferTimer()   // 开启跳跃缓冲计时
    {
        StopCoroutine(nameof(JumpInputBufferCoroutine));
        StartCoroutine(nameof(JumpInputBufferCoroutine));
    }

    private IEnumerator JumpInputBufferCoroutine()
    {
        HasJumpInputBuffer = true;
        yield return _waitJumpInputBuffer;
        HasJumpInputBuffer = false;
    }
}
