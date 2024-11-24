using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HPlayerSkill1State : HPlayerBaseState
{
    private HCharacterSkillBase skillScipt;
    //private bool isSkill1Using = false;
    public HPlayerSkill1State(HPlayerStateMachine currentContext, HPlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        skillScipt = _ctx.gameObject.GetComponent<HCharacterSkillBase>();
    }

    public override void EnterState()
    {
        _ctx.Animator.SetTrigger(_ctx.IsSkill1Hash);
        //找到它所触发的这个动作的时长
        float skill1Duration = _ctx.Animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log("skill1Duration: " + skill1Duration);
        DOVirtual.DelayedCall(skill1Duration, () =>
        {
            SwitchState(_factory.Idle());
        });
        string characterName = _ctx.gameObject.name;
        Debug.Log("characterName: " + characterName);
        
        
        //HAudioManager.Instance.Play("PlayerSkill1", _ctx.gameObject);
        
        //skillScipt.PlaySkill1();
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
