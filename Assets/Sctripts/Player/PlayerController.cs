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

    // ���
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;
    private PlayerAnimation playerAnimation;

    // ���������
    public PlayerInputControl inputControl;

    // �ƶ�����
    [Header("�ƶ�����")]
    public Vector2 inputDirection;
    public float speed;
    private float runSpeed;
    private float walkSpeed => runSpeed / 2.5F;
    public float jumpForce;

    // �¶ײ���
    private Vector2 originalOffset;
    private Vector2 originalSize;
    private Vector2 crouchOffset;
    private Vector2 crouchSize;

    // ��������
    [Header("��������")]
    public int buyao;

    // ���˲���
    [Header("���˲���")]
    public float hurtForce;



    /*------------------------------ Ĭ�Ϻ��� ------------------------------*/
    private void Awake()
    {
        // ��ȡ���
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();

        // ������ֵ
        runSpeed = speed;

        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;
        crouchOffset = new Vector2(-0.05F, 0.85F);
        crouchSize = new Vector2(0.7F, 1.7F);

        // ʵ����
        inputControl = new PlayerInputControl();

        // ע��Input System����
        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.WalkButton.performed += cxt =>
        {
            if(physicsCheck.isGround)
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
        // ��ȡ������Ϣ
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


    /*------------------------------ �Զ��庯�� ------------------------------*/

    public void Move()
    {
        // �����ƶ�
        if(!isCrouch)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }

        // ���﷭ת
        Vector3 tempScale = transform.localScale;
        if (inputDirection.x * tempScale.x < 0F)
        {
            transform.localScale = new Vector3(-tempScale.x, tempScale.y, tempScale.x);
        }

        // �����¶�
        isCrouch = inputDirection.y < -0.5F && physicsCheck.isGround;
        if(isCrouch)
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

    /*------------------------------ Input System ע�ắ�� ------------------------------*/
    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        // ��Ծ���ɹ���
        if(!physicsCheck.isGround)
            return;

        rb.velocity = Vector2.zero;
        isAttack = true;
        playerAnimation.PlayerAttack();
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
}
