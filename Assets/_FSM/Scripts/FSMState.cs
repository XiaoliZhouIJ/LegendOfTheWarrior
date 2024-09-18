using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMIdleState : IState
{
    private FSM manager;
    private Paramter paramter;

    public FSMIdleState(FSM manager)
    {
        this.manager = manager;
        this.paramter = manager.paramter;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
}
