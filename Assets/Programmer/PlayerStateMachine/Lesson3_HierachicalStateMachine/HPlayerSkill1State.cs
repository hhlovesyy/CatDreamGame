using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HPlayerSkill1State : HPlayerBaseState
{
    private HCharacterSkillBase skillScipt;

    private bool isSkillFinished = false;
    //private bool isSkill1Using = false;
    public HPlayerSkill1State(HPlayerStateMachine currentContext, HPlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        skillScipt = _ctx.gameObject.GetComponent<HCharacterSkillBase>();
    }

    public override void EnterState()
    {
        // Debug.Log("HPlayerSkill1State");
        _ctx.Animator.SetTrigger(_ctx.IsSkill1Hash);
        _ctx.SetInputActionDisableOrEnable(true);
        _ctx.AppliedMovementX = 0;
        _ctx.AppliedMovementZ = 0;
        isSkillFinished = false;
        
        // Debug.Log("HPlayerSkill1State: LLLLLLLock" );
        
        //找到它所触发的这个动作的时长
        //float skill1Duration = _ctx.Animator.GetCurrentAnimatorStateInfo(0).length;
        // float skill1Duration = 2.6f;
        float skill1Duration = 1f;
        //Debug.Log("skill1Duration: " + skill1Duration);
        
        EventManager.DispatchEvent<CatSkillStatusEvent, float>(GameEvent.CHANGE_SKILL_STATUS.ToString(), CatSkillStatusEvent.RELEASE_SKILL, skill1Duration);
        DOVirtual.DelayedCall(skill1Duration, () =>
        {
            
            EndSkill1();
        });
        // string characterName = _ctx.gameObject.name;
        // Debug.Log("characterName: " + characterName);
        
        
        //HAudioManager.Instance.Play("PlayerSkill1", _ctx.gameObject);
        
        //skillScipt.PlaySkill1();
    }
    
    void EndSkill1()
    {
        EventManager.DispatchEvent<CatSkillStatusEvent, float>(GameEvent.CHANGE_SKILL_STATUS.ToString(), CatSkillStatusEvent.SKILL_RESUME, 5f);
        _ctx.SetInputActionDisableOrEnable(false);
        //Debug.Log("HPlayerSkill1State: UUUUUUUUNLLLLLLLock" );
        //SwitchState(_factory.Idle());
        isSkillFinished = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Debug.Log("===========skill1Update========");
        Debug.Log("currentState" + _ctx.CurrentState);
        // _currentSuperState
        Debug.Log("currentSuperState" + _currentSuperState);
        //substate
        Debug.Log("currentSubState" + _currentSubState);
       
    }
    
    public override void ExitState()
    {
        
    }
    
    public override void CheckSwitchStates()
    {
        if (isSkillFinished)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
        
    }
}
