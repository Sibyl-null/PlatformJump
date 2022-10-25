using UnityEngine;

public class PlayerGroundedDetector : MonoBehaviour
{
    [SerializeField] private Vector3 halfExtents = 0.1f * Vector3.one;   //检测半径
    [SerializeField] private LayerMask groundLayer;   //检测的地面层

    private Collider[] _colliders = new Collider[1];

    //是否接触地面
    public bool IsGrounded =>
        Physics.OverlapBoxNonAlloc(transform.position, halfExtents, _colliders, Quaternion.identity, groundLayer) != 0;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, halfExtents);
    }
}
