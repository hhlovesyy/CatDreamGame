using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class YPlayerSkill2State : HPlayerBaseState
{
    private HCharacterSkillBase skillScipt;
    //private bool isSkill1Using = false;
    public YPlayerSkill2State(HPlayerStateMachine currentContext, HPlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        skillScipt = _ctx.gameObject.GetComponent<HCharacterSkillBase>();
    }

    public override void EnterState()
    {
        // Debug.Log("HPlayerSkill1State");
        _ctx.Animator.SetTrigger(_ctx.IsSkill2Hash);
        _ctx.SetInputActionDisableOrEnable(true);
        _ctx.AppliedMovementX = 0;
        _ctx.AppliedMovementZ = 0;
        
        // Debug.Log("HPlayerSkill1State: LLLLLLLock" );
        
        //找到它所触发的这个动作的时长
        //float skill1Duration = _ctx.Animator.GetCurrentAnimatorStateInfo(0).length;
        // float skill1Duration = 2.6f;
        float skill2Duration = 2.6f;
        Debug.Log("skill2Duration: " + skill2Duration);
        EventManager.DispatchEvent<CatSkillStatusEvent, float>(GameEvent.CHANGE_SKILL_STATUS.ToString(), CatSkillStatusEvent.RELEASE_SKILL, skill2Duration);
        DOVirtual.DelayedCall(skill2Duration, () =>
        {
            
            EndSkill2();
        });
        // string characterName = _ctx.gameObject.name;
        // Debug.Log("characterName: " + characterName);
        
        
        //HAudioManager.Instance.Play("PlayerSkill1", _ctx.gameObject);
        
        //skillScipt.PlaySkill1();
    }
    
    void EndSkill2()
    {
        EventManager.DispatchEvent<CatSkillStatusEvent, float>(GameEvent.CHANGE_SKILL_STATUS.ToString(), CatSkillStatusEvent.SKILL_RESUME, 5f);
        _ctx.SetInputActionDisableOrEnable(false);
        Debug.Log("HPlayerSkill2State: UUUUUUUUNLLLLLLLock" );
        SwitchState(_factory.Idle());
    }

    public override void UpdateState()
    {
        // CheckSwitchStates();
    }
    
    public override void ExitState()
    {
        
    }
    
    public override void CheckSwitchStates()
    {
        
    }

    public override void InitializeSubState()
    {
        
    }
}
