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
    [Header("��������")]
    public int health;

    [Header("���ò���")]
    public float idleTime;

    [Header("�ƶ�����")]
    public float walkSpeed;
    public float chaseSpeed;
    public Transform[] patroalPoints;
    public Transform[] chasePoints;

    // ������
    [HideInInspector] public Animator animator;
}

public class FSM : MonoBehaviour
{
    public Paramter paramter;

    private IState currentState;
    private Dictionary<FSMState, IState> states = new Dictionary<FSMState, IState>();

    /* ------------------------------ Ĭ�����ں��� ------------------------------ */
    private void Start()
    {
        // ע������״̬
        states.Add(FSMState.Idle, new FSMIdleState(this));

        // ��ȡ���
        paramter.animator = GetComponent<Animator>();

        // ��ʼ״̬ΪIdle
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

    /* ------------------------------ ���˺��� ------------------------------ */
    /// <summary>
    /// �л�״̬��״̬
    /// </summary>
    /// <param name="state">Ŀ��״̬</param>
    public void SwitchState(FSMState state)
    {
        currentState?.OnExit();

        currentState = states[state];

        currentState.OnEnter();
    }
}
