using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    // ���
    private CapsuleCollider2D capsuleCollider;

    [Header("���״̬")]
    public bool manuralCheck;
    public bool isGround;
    public bool isTouchLeftWall;
    public bool isTouchRightWall;

    [Header("���桢ǽ�������")]
    public Vector2 bottonOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayer;


    private void Awake()
    {
        // ��ȡ���
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
        // ������
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottonOffset, checkRadius, groundLayer);

        // ���ǽ��
        isTouchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        isTouchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, groundLayer);


        // ���ǽ��
        // isOnWall
    }

    /// <summary>
    /// OnDrawGizmos            ��������
    /// OnDrawGizmosSelected    ���屻ѡ��ʱ����
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // ������Gizmo
        Gizmos.DrawWireSphere((Vector2)transform.position + bottonOffset, checkRadius);
        // ǽ����Gizmo
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
