using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HMessageShowExp : MonoBehaviour
{

    private void TestMessage5ConfirmAction()
    {
        Debug.Log("Confirm!");
        HMessageShowMgr.Instance.ShowMessage("ROGUE_GACHA_NOENOUGH_XINGQIONG", "点击确认!");
    }
    
    private void TestMessage5CancelAction()
    {
        Debug.Log("Cancel!");
        HMessageShowMgr.Instance.ShowMessage("ROGUE_GACHA_NOENOUGH_XINGQIONG", "点击取消!");
    }
    
    private void TestMessage5CloseAction()
    {
        Debug.Log("Close!");
        HMessageShowMgr.Instance.ShowMessage("ROGUE_GACHA_NOENOUGH_XINGQIONG", "点击关闭!");
    }
    
    
    
    private void OnGUI()
    {
        // 有八个GUI Button，分别对应八个回调函数，名字叫做测试消息类型1~8
        if (GUI.Button(new Rect(10, 10, 300, 200), "测试消息类型1"))
        {
            HMessageShowMgr.Instance.ShowMessage("LEVEL_IN_MSG_0"); //在策划表里，这个类型是1
            HMessageShowMgr.Instance.ShowMessage("LEVEL_LOCATE_MSG_1");
        }
        if (GUI.Button(new Rect(10, 220, 300, 200), "测试消息类型2"))
        {
            HMessageShowMgr.Instance.ShowMessage("Tut_LearnLevel0"); //在策划表里，这个类型是2
        }
        if (GUI.Button(new Rect(10, 430, 300, 200), "测试消息类型4,3还没写完,不支持"))
        {
            HMessageShowMgr.Instance.ShowMessage("SlotMachineSubmitMsg"); //在策划表里，这个类型是3
        }
        if (GUI.Button(new Rect(320, 10, 300, 200), "测试消息类型5"))
        {
            HMessageShowMgr.Instance.ShowMessageWithActions("ConfirmPuppetAction", TestMessage5ConfirmAction, TestMessage5CancelAction, TestMessage5CloseAction);
        }
        if(GUI.Button(new Rect(320, 220, 300, 200), "测试消息类型9"))
        {
            HMessageShowMgr.Instance.ShowMessage("ROGUE_MUSICGAME_LOAD");
        }
        if(GUI.Button(new Rect(320, 430, 300, 200), "测试消息类型10"))
        {
            HMessageShowMgr.Instance.ShowMessage("SUNDAY_BOSS_DAY1");
        }
        if(GUI.Button(new Rect(630, 10, 300, 200), "测试消息类型11"))
        {
            HMessageShowMgr.Instance.ShowMessageOnOrOff("SHOW_KEYBOARD_CTRL", true);
            DOVirtual.DelayedCall(2f, () =>
            {
                HMessageShowMgr.Instance.ShowMessageOnOrOff("SHOW_KEYBOARD_CTRL", false);
            });
        }
    }
}
