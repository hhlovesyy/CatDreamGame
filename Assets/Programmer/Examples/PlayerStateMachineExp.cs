using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachineExp : MonoBehaviour
{
    public Transform player;

    private HPlayerStateMachine stateMachine;

    private void Start()
    {
        stateMachine = player.gameObject.GetComponent<HPlayerStateMachine>();
        if (!stateMachine)
        {
            Debug.LogError("PlayerStateMachineExp: No HPlayerStateMachine found on player");
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 200), "测试角色死亡"))
        {
            stateMachine.IsDie = true;
        }
        if (GUI.Button(new Rect(10, 230, 300, 200), "测试角色倒转乾坤"))
        {
            stateMachine.OnStandingIdle();
        }
    }
}
