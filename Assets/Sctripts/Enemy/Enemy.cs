using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 组件
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PhysicsCheck physicsCheck;
    private CapsuleCollider2D capsuleCollider;

    [Header("状态")]
    public bool isIdle;
    public bool isHit;
    public bool isDead;

    [Header("移动参数")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;

    [Header("闲置参数")]
    public float idleTime;

    [Header("丢失目标")]
    public float lostTime;


    [Header("受击参数")]
    public Transform attacker;
    public float hitForce;

    [Header("有限状态机")]
    [HideInInspector] public BaseState currentState;
    [HideInInspector] public BaseState idleState;
    [HideInInspector] public BaseState patrolState;
    [HideInInspector] public BaseState chaseState;
    [HideInInspector] public BaseState skillState;

    [Header("玩家检测")]
    public Vector2 centerOffset;
    public Vector2 checkBoxSize;
    // public Vector3 faceDir;      // 已存在
    public float checkBoxDistance;
    public LayerMask attackLayerMask;

    /* ------------------------------ 默认周期函数 ------------------------------ */

    protected virtual void Awake()
    {
        // 获取组件
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();    
        physicsCheck = GetComponent<PhysicsCheck>();

        // 参数赋值
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

    /* ------------------------------ 角色行动 ------------------------------ */
    /// <summary>
    /// Move()
    /// CharactorTurnAround()
    /// OntakeDamage()
    /// OnDie()
    /// </summary> 
    // 
    #region 角色行动
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
        // 转身
        if (dir.x * faceDir.x > 0)
        {
            CharactorTurnAround();
        }

        // 击退
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