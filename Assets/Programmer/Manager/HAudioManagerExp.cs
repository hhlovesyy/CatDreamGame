using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAudioManagerExp : MonoBehaviour
{
    public Transform player; //示例代码,这里假设有一个player,后面player也可以通过脚本获取
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 200), "测试正常播放音乐1"))
        {
            HAudioManager.Instance.Play("BeginGameMusic", this.gameObject);
        }
        if (GUI.Button(new Rect(10, 220, 300, 200), "测试玩家播放音效"))
        {
            HAudioManager.Instance.Play("RunFootStepOnGround", player.gameObject);
        }
        if (GUI.Button(new Rect(10, 430, 300, 200), "测试从某一节点开始播放音乐"))
        {
            HAudioManager.Instance.Play("RobinCatcakeBattleAudio", player.gameObject, 85.3f);
        }
        //换到下一列
        if (GUI.Button(new Rect(320, 10, 300, 200), "测试停止音乐"))
        {
            HAudioManager.Instance.Stop(player.gameObject);
        }
        if (GUI.Button(new Rect(320, 220, 300, 200), "测试淡出音乐"))
        {
            HAudioManager.Instance.EaseOutAndStop(player.gameObject);
        }
    }
}
