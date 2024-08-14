using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YTest_CountDown  : MonoBehaviour
{
    //
    YCountDownUI countDownUI;
    private void Start()
    {
        countDownUI = gameObject.AddComponent<YCountDownUI>();
        // countDownUI.addCountDownUIlink = "RecallCountDownPanel";
        // countDownUI.skillLastTime = duration ;
    }

    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle(GUI.skin.button);
        fontStyle.fontSize = 30;
        if (GUI.Button(new Rect(10, 10, 300, 200), "测试倒计时显示",fontStyle))
        {
            countDownUI.Showicon(true);
        }
        if (GUI.Button(new Rect(10, 220, 300, 200), "测试倒计时不显示",fontStyle))
        {
            countDownUI.Showicon(false);
        }
        if (GUI.Button(new Rect(10, 430, 300, 200), "测试倒计时开始",fontStyle))
        {
            countDownUI.BeginCountDown();
        }
        //换到下一列
        if (GUI.Button(new Rect(10, 630, 300, 200), "测试倒计时结束",fontStyle))
        {
            countDownUI.EndCountDown();
        }
        
    }
}
