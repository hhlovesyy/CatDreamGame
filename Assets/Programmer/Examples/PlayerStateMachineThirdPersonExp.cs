using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachineThirdPersonExp : MonoBehaviour
{
    public Transform player;

    private HPlayerStateMachine stateMachine;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = player.gameObject.GetComponent<HPlayerStateMachine>();
        if (!stateMachine)
        {
            Debug.LogError("PlayerStateMachineExp: No HPlayerStateMachine found on player");
            return;
        }
        stateMachine.SetInThirdPersonCamera(true); //这个是设置第三人称的固定相机
    }
}
