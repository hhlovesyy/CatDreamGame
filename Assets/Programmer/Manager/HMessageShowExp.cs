using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMessageShowExp : MonoBehaviour
{
    private void OnGUI()
    {
        // 有八个GUI Button，分别对应八个回调函数，名字叫做测试消息类型1~8
        if (GUI.Button(new Rect(10, 10, 300, 200), "测试消息类型1"))
        {
            HMessageShowMgr.Instance.ShowMessage("LEVEL_IN_MSG_0"); //在策划表里，这个类型是1
            HMessageShowMgr.Instance.ShowMessage("LEVEL_LOCATE_MSG_1");
        }
        if (GUI.Button(new Rect(10, 420, 350, 200), "测试消息类型2"))
        {
            HMessageShowMgr.Instance.ShowMessage("Tut_LearnLevel0"); //在策划表里，这个类型是2
        }
    }
}
