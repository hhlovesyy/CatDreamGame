// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// namespace CatGameStateMachine
// {
// public abstract class YCatBaseState
// {
//     protected YCatStateMachine _ctx;
//     protected YCatStateFactory _factory;
//     
//     protected YCatBaseState _currentSubState;
//     protected YCatBaseState _currentSuperState;
//
//     protected bool _isRootState = false;
//     
//     public YCatBaseState(YCatStateMachine currentContext, YCatStateFactory playerStateFactory)
//     {
//         _ctx = currentContext;
//         _factory = playerStateFactory;
//     }
//     
//     public abstract void EnterState();
//     public abstract void UpdateState();
//     public abstract void ExitState();
//     public abstract void CheckSwitchStates();
//     public abstract void InitializeSubState();
//
//     public void UpdateStates()
//     {
//         UpdateState();
//         if (_currentSubState != null)
//         {
//             _currentSubState.UpdateStates();
//         }
//     }
//
//     // public void ExitStates()
//     // {
//     //     ExitState();
//     //     if(_currentSubState != null)
//     //     {
//     //         _currentSubState.ExitStates();
//     //     }
//     // }
//
//     protected void SwitchState(YCatBaseState newState)
//     {
//         //current state exits state
//         ExitState();
//         
//         //new state enters state
//         newState.EnterState();
//
//         if (_isRootState)
//         {
//             //switch current state of context
//             _ctx.CurrentState = newState;
//         } else if (_currentSuperState != null)
//         {
//             //switch current substate of superstate
//             _currentSuperState.SetSubState(newState);
//         }
//         
//     }
//
//     protected void SetSuperState(YCatBaseState newSuperstate)
//     {
//         _currentSuperState = newSuperstate;
//     }
//
//     protected void SetSubState(YCatBaseState newSubstate)
//     {
//         _currentSubState = newSubstate;
//         newSubstate.SetSuperState(this);
//     }
//
// }
// }