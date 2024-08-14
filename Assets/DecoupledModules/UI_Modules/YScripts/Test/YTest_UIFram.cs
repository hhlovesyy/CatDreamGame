using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YTest_UIFram : MonoBehaviour
{
   
    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle(GUI.skin.button);
        fontStyle.fontSize = 30;
        if (GUI.Button(new Rect(10, 10, 300, 200), "测试外部增加 Settingpanel",fontStyle))
        {
            YGameRoot.Instance.Push(new SettingPanel());
        }
        if (GUI.Button(new Rect(10, 220, 300, 200), "测试1",fontStyle))
        {
            //countDownUI.Showicon(false);
        }
        if (GUI.Button(new Rect(10, 430, 300, 200), "测试1",fontStyle))
        {
            //countDownUI.BeginCountDown();
        }
        //换到下一列
        if (GUI.Button(new Rect(10, 630, 300, 200), "测试1",fontStyle))
        {
            //countDownUI.EndCountDown();
        }
        
    }
}
