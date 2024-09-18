using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ���
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;

    [Header("״̬")]
    public bool isIdle;
    public bool isHit;
    public bool isDead;

    [Header("�ƶ�����")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;

    [Header("���ò���")]
    public float idleTime;

    [Header("��ʧĿ��")]
    public float lostTime;


    [Header("�ܻ�����")]
    public Transform attacker;
    public float hitForce;

    [Header("����״̬��")]
    [HideInInspector] public BaseState currentState;
    [HideInInspector] public BaseState idleState;
    [HideInInspector] public BaseState patrolState;
    [HideInInspector] public BaseState chaseState;
    [HideInInspector] public BaseState skillState;

    [Header("��Ҽ��")]
    public Vector2 centerOffset;
    public Vector2 checkBoxSize;
    // public Vector3 faceDir;      // �Ѵ���
    public float checkBoxDistance;
    public LayerMask attackLayerMask;

    /* ------------------------------ Ĭ�����ں��� ------------------------------ */

    protected virtual void Awake()
    {
        // ��ȡ���
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();    
        physicsCheck = GetComponent<PhysicsCheck>();

        // ������ֵ
        currentSpeed = normalSpeed;
    }

    private void OnEnable()
    {
        currentState = idleState;
        currentState.OnEnter(this);
    }

    private void OnDisable()
    {
        currentState.OnExit();
        currentState = null;
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0F, 0F).normalized;

        currentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    /* ------------------------------ ��ɫ�ж� ------------------------------ */
    /// <summary>
    /// Move()
    /// CharactorTurnAround()
    /// OntakeDamage()
    /// OnDie()
    /// </summary> 
    // 
    #region ��ɫ�ж�
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    public void CharactorTurnAround()
    {
        transform.localScale = new Vector3(faceDir.x, 1F, 1F);
        physicsCheck.OffsetCheck();
    }

    public void OnTakeDamage(Transform attackerTrans)
    {
        attacker = attackerTrans;
        Vector2 dir = new Vector2(transform.position.x - attacker.transform.position.x, 0).normalized;
        // ת��
        if (dir.x * faceDir.x > 0)
        {
            CharactorTurnAround();
        }

        // ����
        isHit = true;
        animator.SetTrigger("hit");

        StartCoroutine(OnHit(dir));
    }

    public void OnDie()
    {
        animator.SetTrigger("dead");
        GetComponent<Attack>().enabled = false;
    }

    private IEnumerator OnHit(Vector2 dir)
    {
        rb.AddForce(dir * hitForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1F);

        isHit = false;
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }

    public void SwitchState(EnemyState eState)
    {
        

        BaseState newState = eState switch
        {
            EnemyState.Idle => idleState,
            EnemyState.Patrol => patrolState,
            EnemyState.Chase => chaseState,
            EnemyState.Skill => skillState,
            _ => null
        } ;

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public bool IsFoundPlayer()
    {
        return Physics2D.BoxCast(   (Vector2)transform.position + centerOffset, 
                                    checkBoxSize, 
                                    0F, 
                                    faceDir, 
                                    checkBoxDistance, 
                                    attackLayerMask);

        
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + centerOffset + new Vector2(checkBoxDistance * faceDir.x, 0F), checkBoxSize);
        // Gizmos.DrawWireSphere((Vector2)transform.position + centerOffset, 0.1F);
    }
}