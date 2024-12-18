using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : HPlayerBaseState
{
    public PlayerWalkState(HPlayerStateMachine currentContext, HPlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        
    }
    public override void EnterState()
    {
        if (!_ctx.IsInThirdPersonCamera)
        {
            _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);
            _ctx.Animator.SetBool(_ctx.IsRunningHash, false);
        }
        //HAudioManager.instance.Play("FootStepOnGround", this._ctx.gameObject);
    }

    public override void InitializeSubState()
    {
        
    }

    public override void ExitState()
    {
        //_ctx.Animator.SetBool(_ctx.IsWalkingHash, false);
    }

    public override void CheckSwitchStates()
    {
        if(!_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Idle());
        }
        else if(_ctx.IsMovementPressed && _ctx.IsRunPressed)
        {
            SwitchState(_factory.Run());
        }
        else if (_ctx.IsSkill1Pressed)
        {
            SwitchState(_factory.Skill1());
        }
        else if (_ctx.IsSkill2Pressed)
        {
            SwitchState(_factory.Skill2());
        }
        else if (_ctx.IsSkill3Pressed)
        {
            SwitchState(_factory.Skill3());
        }
    }

    public override void UpdateState()
    {
        _ctx.AppliedMovementX = _ctx.CurrentMovementInput.x * 2;
        _ctx.AppliedMovementZ = _ctx.CurrentMovementInput.y * 2;
        CheckSwitchStates();
    }
}
