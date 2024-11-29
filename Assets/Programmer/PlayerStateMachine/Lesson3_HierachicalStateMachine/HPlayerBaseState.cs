using UnityEngine;

public abstract class HPlayerBaseState
{
    protected HPlayerStateMachine _ctx;
    protected HPlayerStateFactory _factory;
    
    protected HPlayerBaseState _currentSubState;
    protected HPlayerBaseState _currentSuperState;

    protected bool _isRootState = false;
    
    public HPlayerBaseState(HPlayerStateMachine currentContext, HPlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    // public void ExitStates()
    // {
    //     ExitState();
    //     if(_currentSubState != null)
    //     {
    //         _currentSubState.ExitStates();
    //     }
    // }

    protected void SwitchState(HPlayerBaseState newState)
    {
        //Log 时间戳和当前状态
        // Debug.Log("===================================================");
        // Debug.Log("Switching from " + this + " to " + newState + " at " + Time.time);
        // Debug.Log("superstate is " + _currentSuperState);
        // Debug.Log("substate is " + _currentSubState);
        
        //current state exits state
        ExitState();
        
        //new state enters state
        newState.EnterState();

        if (_isRootState)
        {
            //switch current state of context
            _ctx.CurrentState = newState;
        } else if (_currentSuperState != null)
        {
            //switch current substate of superstate
            _currentSuperState.SetSubState(newState);
        }
        else
        {
            Debug.Log("bad cat");
        }
        
    }

    protected void SetSuperState(HPlayerBaseState newSuperstate)
    {
        _currentSuperState = newSuperstate;
    }

    protected void SetSubState(HPlayerBaseState newSubstate)
    {
        _currentSubState = newSubstate;
        newSubstate.SetSuperState(this);
    }

}
