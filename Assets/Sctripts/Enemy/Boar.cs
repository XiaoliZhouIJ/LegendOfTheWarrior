using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new BoarIdleState();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }
    //public override void Move()
    //{
    //    base.Move();
    //    // ×ßÂ·
    //    animator.SetBool("isWalk", true);
    //    animator.SetBool("isRun", false);
    //}
}
