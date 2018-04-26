using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public FSM fsm;

    public State(FSM fsm)
    {
        this.fsm = fsm;
    }

    public virtual void EnterState()
    {

    }

    public virtual void UpdateState()
    {

    }

    public virtual void FixedUpdateState()
    {

    }

    public virtual void ExitState()
    {

    }
}

public class FSM {
    private Dictionary<string, State> stateList = new Dictionary<string, State>();
    private State currentState;

    /// <summary>
    /// 注册状态
    /// </summary>
    /// <param name="stateName">状态名</param>
    /// <param name="state">状态</param>
    /// <returns>注册成功返回true</returns>
    public bool RegisterState(string stateName, State state)
    {
        if (!stateList.ContainsKey(stateName))
        {
            stateList.Add(stateName, state);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    /// <param name="defaultStateName"></param>
    public void StartStateMachine(string defaultStateName)
    {
        if (stateList.ContainsKey(defaultStateName))
        {
            currentState = stateList[defaultStateName];
        }
        currentState.EnterState();
    }

    /// <summary>
    /// Update运行状态机
    /// </summary>
    public void Update()
    {
        currentState.UpdateState();
    }

    /// <summary>
    /// FixedUpdate运行状态机
    /// </summary>
    public void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="nextStateName">下一个状态名</param>
    /// <param name="canTransitionToSelf">能否重复切换到相同状态</param>
    public void SwitchState(string nextStateName, bool canTransitionToSelf = false)
    {
        State nextState = stateList[nextStateName];
        if (canTransitionToSelf)
        {
            currentState.ExitState();
            currentState = nextState;
            currentState.EnterState();
        }
        else if (nextState != currentState)
        {
            currentState.ExitState();
            currentState = nextState;
            currentState.EnterState();
        }
    }
}
