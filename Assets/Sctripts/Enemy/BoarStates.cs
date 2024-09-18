using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÏÐÖÃ×´Ì¬
/// </summary>
public class BoarIdleState : BaseState
{
    private float idleTimer;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

        idleTimer = currentEnemy.idleTime;
        currentEnemy.rb.velocity = Vector2.zero;

        currentEnemy.isIdle = true;
        currentEnemy.animator.SetBool("isWalk", false);
        currentEnemy.animator.SetBool("isRun", false);
    }

    public override void OnUpdate()
    {
        if (RunIdleTimer())             // Idle×´Ì¬½áÊø£¬ÇÐ»»ÎªPatrol×´Ì¬
        {
            // ÇÐ»»×´Ì¬
            currentEnemy.SwitchState(EnemyState.Patrol);
        }

    }

    public override void OnFixedUpdate()
    {
        if (currentEnemy.IsFoundPlayer())
        {
            currentEnemy.SwitchState(EnemyState.Chase);
        }
    }

    public override void OnExit()
    {
        // ÉèÖÃ¶ÔÓ¦²ÎÊý
        currentEnemy.isIdle = false;
        currentEnemy.rb.velocity = Vector2.zero;
        currentEnemy.animator.SetBool("isWalk", false);
        currentEnemy.animator.SetBool("isRun", false);
    }

    public bool RunIdleTimer()
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0)
        {
            idleTimer = 0;
        }

        return idleTimer == 0;
    }
}

/// <summary>
/// Ñ²Âß×´Ì¬
/// </summary>
public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

        currentEnemy.currentSpeed = currentEnemy.normalSpeed;

        currentEnemy.animator.SetBool("isWalk", true);
        currentEnemy.animator.SetBool("isRun", false);
    }

    public override void OnUpdate()
    {
        

        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.isTouchLeftWall)
        {
            // ÍË³öÇ°×ªÉí
            currentEnemy.CharactorTurnAround();

            currentEnemy.SwitchState(EnemyState.Idle);
        }

        if (currentEnemy.IsFoundPlayer())
        {
            currentEnemy.SwitchState(EnemyState.Chase);
        }
    }

    public override void OnFixedUpdate()
    {
        currentEnemy.Move();
    }

    public override void OnExit()
    {
        currentEnemy.rb.velocity = Vector2.zero;
        currentEnemy.animator.SetBool("isWalk", false);
        currentEnemy.animator.SetBool("isRun", false);
    }
}

/// <summary>
/// ×·»÷×´Ì¬
/// </summary>
public class BoarChaseState : BaseState
{
    public float lostTimer;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

        lostTimer = currentEnemy.lostTime;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;

        currentEnemy.animator.SetBool("isWalk", false);
        currentEnemy.animator.SetBool("isRun", true);
    }

    public override void OnUpdate()
    {
        

        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.isTouchLeftWall)
        {
            currentEnemy.CharactorTurnAround();
        }

        if(!currentEnemy.IsFoundPlayer())
        {
            if(RunLostTimer())          // ¼ÆÊ±Æ÷½áÊø£¬ÍË»ØIdle×´Ì¬
            {
                currentEnemy.SwitchState(EnemyState.Idle);
            }
        }
    }

    public override void OnFixedUpdate()
    {
        currentEnemy.Move();
    }

    public override void OnExit()
    {
        currentEnemy.rb.velocity = Vector2.zero;
        currentEnemy.animator.SetBool("isWalk", false);
        currentEnemy.animator.SetBool("isRun", false);
    }

    public bool RunLostTimer()
    {
        lostTimer -= Time.deltaTime;
        if (lostTimer <= 0)
        {
            lostTimer = 0;
        }

        return lostTimer == 0;
    }
}

/// <summary>
/// ÊÜ»÷×´Ì¬
/// </summary>
public class BoarHitState : BaseState
{
    public float lostTimer;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

        lostTimer = currentEnemy.lostTime;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;

        currentEnemy.animator.SetBool("isWalk", false);
        currentEnemy.animator.SetBool("isRun", true);
    }

    public override void OnUpdate()
    {
        currentEnemy.Move();

        if (!currentEnemy.physicsCheck.isGround || currentEnemy.physicsCheck.isTouchLeftWall)
        {
            currentEnemy.CharactorTurnAround();
        }

        if (!currentEnemy.IsFoundPlayer())
        {
            if (RunLostTimer())          // ¼ÆÊ±Æ÷½áÊø£¬ÍË»ØIdle×´Ì¬
            {
                currentEnemy.SwitchState(EnemyState.Idle);
            }
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.rb.velocity = Vector2.zero;
        currentEnemy.animator.SetBool("isWalk", false);
        currentEnemy.animator.SetBool("isRun", false);
    }

    public bool RunLostTimer()
    {
        lostTimer -= Time.deltaTime;
        if (lostTimer <= 0)
        {
            lostTimer = 0;
        }

        return lostTimer == 0;
    }
}
