using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 状态信息
    [Header("状态")]
    public bool isCrouch;
    public bool isAttack;
    //public bool isOnWall;
    public bool isHurt;
    public bool isDead;

    // 组件
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private PlayerAnimation playerAnimation;

    // 输入控制器
    public PlayerInputControl inputControl;

    // 移动参数
    [Header("移动参数")]
    public Vector2 inputDirection;
    public float speed;
    private float runSpeed;
    private float walkSpeed => runSpeed / 2.5F;
    public float jumpForce;

    // 下蹲参数
    private Vector2 originalOffset;
    private Vector2 originalSize;
    private Vector2 crouchOffset;
    private Vector2 crouchSize;

    // 攻击参数
    [Header("攻击参数")]
    public int buyao;

    // 受伤参数
    [Header("受伤参数")]
    public float hurtForce;



    /*------------------------------ 默认函数 ------------------------------*/
    private void Awake()
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        // 参数赋值
        runSpeed = speed;

        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;
        crouchOffset = new Vector2(-0.05F, 0.85F);
        crouchSize = new Vector2(0.7F, 1.7F);

        // 实例化
        inputControl = new PlayerInputControl();

        // 注册Input System函数
        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.WalkButton.performed += cxt =>
        {
            if(physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };          // Walk Button 按下后切换到 **走路状态**
        inputControl.Gameplay.WalkButton.canceled += cxt =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };           // Walk Button 松开后切换回 **跑步状态**
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }



    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        // 读取输入信息
        if (inputControl != null)
        {
            inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        if (inputControl == null) return;
        
        if (!isHurt && !isAttack)
        {
            Move();
        }
    }


    /*------------------------------ 自定义函数 ------------------------------*/

    public void Move()
    {
        // 人物移动
        if(!isCrouch)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }

        // 人物翻转
        Vector3 tempScale = transform.localScale;
        if (inputDirection.x * tempScale.x < 0F)
        {
            transform.localScale = new Vector3(-tempScale.x, tempScale.y, tempScale.x);
        }

        // 人物下蹲
        isCrouch = inputDirection.y < -0.5F && physicsCheck.isGround;
        if(isCrouch)
        {
            // 修改碰撞体大小和位移
            capsuleCollider.offset = crouchOffset;
            capsuleCollider.size = crouchSize;
        }
        else
        {
            // 还原碰撞体
            capsuleCollider.offset = originalOffset;
            capsuleCollider.size = originalSize;
        }
    }

    /*------------------------------ Input System 注册函数 ------------------------------*/
    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        // 跳跃不可攻击
        if(!physicsCheck.isGround)
            return;

        rb.velocity = Vector2.zero;
        isAttack = true;
        playerAnimation.PlayerAttack();
    }

    /*------------------------------ Unity Event 事件 ------------------------------*/
    public void PlayerGetHurt(Transform attacker)
    {
        isHurt = true;

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, transform.position.y - attacker.position.y).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
}
