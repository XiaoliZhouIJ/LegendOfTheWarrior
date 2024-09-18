using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制Player动画中的变量
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (animator != null)
        {
            SetAnimation();
        }
    }

    public void SetAnimation()
    {
        animator.SetFloat("velocity_x", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocity_y", rb.velocity.y);
        animator.SetBool("isGround", physicsCheck.isGround);
        animator.SetBool("isCrouch", playerController.isCrouch);
        animator.SetBool("isDead",playerController.isDead);
        animator.SetBool("isAttack",playerController.isAttack);
    }

    public void PlayerFlash()
    {
        animator.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        animator.SetTrigger("attack");
    }
}
