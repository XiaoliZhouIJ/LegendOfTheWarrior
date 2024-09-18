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
    public bool isWallJump;
    public bool isSlide;

    // 组件
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private PlayerAnimation playerAnimation;
    private Character character;

    // 输入控制器
    public PlayerInputControl inputControl;

    // 移动参数
    [Header("移动参数")]
    public Vector2 inputDirection;
    public float speed;
    private float runSpeed;
    private float walkSpeed => runSpeed / 2.5F;
    public float jumpForce;
    public float wallJumpForce;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    // 下蹲参数
    private Vector2 originalOffset;
    private Vector2 originalSize;
    private Vector2 crouchOffset;
    private Vector2 crouchSize;

    // 攻击参数
    [Header("攻击参数")]
    public int NONEED;

    // 受伤参数
    [Header("受伤参数")]
    public float hurtForce;

    // 滑铲参数
    [Header("滑铲参数")]
    public float slideDistance;
    public float slideSpeed;
    public int slidePowerCost;



    /*------------------------------ 默认函数 ------------------------------*/
    private void Awake()
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        character = GetComponent<Character>();

        // 参数赋值
        runSpeed = speed;

        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;
        crouchOffset = new Vector2(-0.05F, 0.85F);
        crouchSize = new Vector2(0.7F, 1.7F);

        // 实例化
        inputControl = new PlayerInputControl();

        // 注册Input System函数
        // 跳跃
        inputControl.Gameplay.Jump.started += Jump;

        // 走路
        inputControl.Gameplay.WalkButton.performed += cxt =>
        {
            if (physicsCheck.isGround)
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

        // 攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;

        // 滑铲
        inputControl.Gameplay.Slide.started += Slide;
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

        CheckState();

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
        if (!isCrouch && !isWallJump && !isSlide)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }

        // 人物翻转
        if(!isSlide)
        {
            Vector3 tempScale = transform.localScale;
            if (rb.velocity.x * tempScale.x < 0F)
            {
                transform.localScale = new Vector3(-tempScale.x, tempScale.y, tempScale.x);
            }
        }


        // 人物下蹲
        isCrouch = inputDirection.y < -0.5F && physicsCheck.isGround;
        if (isCrouch)
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

    private void CheckState()
    {
        capsuleCollider.sharedMaterial = physicsCheck.isGround ? normal : wall;

        if (physicsCheck.isOnWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2F);
        }

        if (isWallJump && rb.velocity.y < 0F)
        {
            isWallJump = false;
        }
    }

    /*------------------------------ Input System 注册函数 ------------------------------*/
    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            StopAllCoroutines();
            isSlide = false;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else if (physicsCheck.isOnWall)
        {
            rb.AddForce((new Vector2(-inputDirection.x, 8F)).normalized * wallJumpForce, ForceMode2D.Impulse);
            isWallJump = true;
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        // 跳跃不可攻击
        if (!physicsCheck.isGround)
            return;

        rb.velocity = Vector2.zero;
        isAttack = true;
        playerAnimation.PlayerAttack();
    }

    private void Slide(InputAction.CallbackContext context)
    {
        if (!isSlide && physicsCheck.isGround && character.OnSlide(slidePowerCost))
        {
            isSlide = true;


            int faceDir = faceDir = rb.velocity.x > 0 ? 1 : -1;

            if (Mathf.Abs(rb.velocity.x) < 0.1F)
            {
                faceDir = transform.localScale.x > 0 ? 1 : -1;
            }

            Vector3 tragetPos = new Vector3(transform.position.x + slideDistance * faceDir, transform.position.y);

            StartCoroutine(TriggerSlide(tragetPos));
        }
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

    /*------------------------------ 携程 ------------------------------*/
    IEnumerator TriggerSlide(Vector3 targetPos)
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        int faceDir = targetPos.x - transform.position.x > 0 ? 1 : -1;

        Debug.Log(faceDir);
        do
        {
            yield return null;
            if(!physicsCheck.isGround)
            {
                // isSlide = false;
                break;
            }

            if(physicsCheck.isTouchLeftWall || physicsCheck.isTouchRightWall)
            {
                // isSlide = false;
                break;
            }



            rb.MovePosition(new Vector2(transform.position.x + faceDir * slideSpeed, transform.position.y));
            // rb.MovePosition(new Vector2(transform.position.x - 0.1F, transform.position.y));

            // rb.velocity = new Vector2(faceDir * slideSpeed,rb.velocity.y);
        } while (MathF.Abs(targetPos.x - transform.position.x) > 0.2F);

        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

    }

}
