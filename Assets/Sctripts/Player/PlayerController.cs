using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    // ״̬��Ϣ
    [Header("״̬")]
    public bool isCrouch;
    public bool isAttack;
    //public bool isOnWall;
    public bool isHurt;
    public bool isDead;
    public bool isWallJump;
    public bool isSlide;

    // ���
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private PlayerAnimation playerAnimation;
    private Character character;

    // ���������
    public PlayerInputControl inputControl;

    // �ƶ�����
    [Header("�ƶ�����")]
    public Vector2 inputDirection;
    public float speed;
    private float runSpeed;
    private float walkSpeed => runSpeed / 2.5F;
    public float jumpForce;
    public float wallJumpForce;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    // �¶ײ���
    private Vector2 originalOffset;
    private Vector2 originalSize;
    private Vector2 crouchOffset;
    private Vector2 crouchSize;

    // ��������
    [Header("��������")]
    public int NONEED;

    // ���˲���
    [Header("���˲���")]
    public float hurtForce;

    // ��������
    [Header("��������")]
    public float slideDistance;
    public float slideSpeed;
    public int slidePowerCost;



    /*------------------------------ Ĭ�Ϻ��� ------------------------------*/
    private void Awake()
    {
        // ��ȡ���
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        character = GetComponent<Character>();

        // ������ֵ
        runSpeed = speed;

        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;
        crouchOffset = new Vector2(-0.05F, 0.85F);
        crouchSize = new Vector2(0.7F, 1.7F);

        // ʵ����
        inputControl = new PlayerInputControl();

        // ע��Input System����
        // ��Ծ
        inputControl.Gameplay.Jump.started += Jump;

        // ��·
        inputControl.Gameplay.WalkButton.performed += cxt =>
        {
            if (physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };          // Walk Button ���º��л��� **��·״̬**
        inputControl.Gameplay.WalkButton.canceled += cxt =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };           // Walk Button �ɿ����л��� **�ܲ�״̬**

        // ����
        inputControl.Gameplay.Attack.started += PlayerAttack;

        // ����
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
        // ��ȡ������Ϣ
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


    /*------------------------------ �Զ��庯�� ------------------------------*/

    public void Move()
    {
        // �����ƶ�
        if (!isCrouch && !isWallJump && !isSlide)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }

        // ���﷭ת
        if(!isSlide)
        {
            Vector3 tempScale = transform.localScale;
            if (rb.velocity.x * tempScale.x < 0F)
            {
                transform.localScale = new Vector3(-tempScale.x, tempScale.y, tempScale.x);
            }
        }


        // �����¶�
        isCrouch = inputDirection.y < -0.5F && physicsCheck.isGround;
        if (isCrouch)
        {
            // �޸���ײ���С��λ��
            capsuleCollider.offset = crouchOffset;
            capsuleCollider.size = crouchSize;
        }
        else
        {
            // ��ԭ��ײ��
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

    /*------------------------------ Input System ע�ắ�� ------------------------------*/
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
        // ��Ծ���ɹ���
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

    /*------------------------------ Unity Event �¼� ------------------------------*/
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

    /*------------------------------ Я�� ------------------------------*/
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
