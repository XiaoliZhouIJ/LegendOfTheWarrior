using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    // 组件
    private CapsuleCollider2D capsuleCollider;

    [Header("检测状态")]
    public bool manuralCheck;
    public bool isGround;
    public bool isTouchLeftWall;
    public bool isTouchRightWall;

    [Header("地面、墙面检测参数")]
    public Vector2 bottonOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;


    private void Awake()
    {
        // 获取组件
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (!manuralCheck )
        {
            leftOffset = new Vector2(-capsuleCollider.bounds.size.x / 2 + capsuleCollider.offset.x, capsuleCollider.offset.y);
            rightOffset = new Vector2( capsuleCollider.bounds.size.x / 2 + capsuleCollider.offset.x, capsuleCollider.offset.y);
        }
    }

    private void Update()
    {
        Check();
    }
    public void Check()
    {
        // 检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottonOffset, checkRadius, groundLayer);

        // 检测墙面
        isTouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        isTouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);


        // 检测墙面
        // isOnWall
    }

    /// <summary>
    /// OnDrawGizmos            持续绘制
    /// OnDrawGizmosSelected    物体被选中时绘制
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // 地面检测Gizmo
        Gizmos.DrawWireSphere((Vector2)transform.position + bottonOffset, checkRadius);
        // 墙面检测Gizmo
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);

    }

    public void OffsetCheck()
    {
        bottonOffset = new Vector2(-bottonOffset.x, bottonOffset.y);
        leftOffset = new Vector2(-leftOffset.x, leftOffset.y);
        rightOffset = new Vector2(-rightOffset.x, rightOffset.y);
    }
}
