using UnityEngine;

// 漂浮状态
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Float", fileName = "PlayerState_Float")]
public sealed class PlayerState_Float : PlayerState
{
    [SerializeField] private EventCenterVoid eventCenterPlayerDefeated;
    [SerializeField] private float floatingSpeed = 0.5f;  //漂浮速度
    [SerializeField] private Vector3 floatingOffset;   //初始漂浮位置

    private Vector3 _floatingTargetPos;
    private Transform _playerTransform;
    
    public override void Enter()
    {
        base.Enter();
        
        eventCenterPlayerDefeated.EventTrigger();
        _playerTransform = playerController.transform;
        _floatingTargetPos = _playerTransform.position + floatingOffset;
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(_floatingTargetPos, _playerTransform.position) > floatingSpeed * Time.deltaTime)
        {
            _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, _floatingTargetPos,
                floatingSpeed * Time.deltaTime);
        }
        else
        {
            _floatingTargetPos += (Vector3)Random.insideUnitCircle;
        }
    }
}
