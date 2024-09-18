using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum FSMState
{
    Idle, Patrol, Chase, React, Attack
}

[Serializable]
public class Paramter
{
    [Header("基础参数")]
    public int health;

    [Header("闲置参数")]
    public float idleTime;

    [Header("移动参数")]
    public float walkSpeed;
    public float chaseSpeed;
    public Transform[] patroalPoints;
    public Transform[] chasePoints;

    // 管理器
    [HideInInspector] public Animator animator;
}

public class FSM : MonoBehaviour
{
    public Paramter paramter;

    private IState currentState;
    private Dictionary<FSMState, IState> states = new Dictionary<FSMState, IState>();

    /* ------------------------------ 默认周期函数 ------------------------------ */
    private void Start()
    {
        // 注册所有状态
        states.Add(FSMState.Idle, new FSMIdleState(this));

        // 获取组件
        paramter.animator = GetComponent<Animator>();

        // 初始状态为Idle
        SwitchState(FSMState.Idle);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }

    /* ------------------------------ 个人函数 ------------------------------ */
    /// <summary>
    /// 切换状态机状态
    /// </summary>
    /// <param name="state">目标状态</param>
    public void SwitchState(FSMState state)
    {
        currentState?.OnExit();

        currentState = states[state];

        currentState.OnEnter();
    }
}
