using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGameFramework;

//enum SliderEvent
public enum SliderEvent 
{
    SLIDER_VALUE_CHANGE, //slider的事件
    SLIDER_UPPERBOUND_CHANGE,
}

public enum GameEvent
{
    CHANGE_SLIDER_VALUE, //改变slider值
}

public class HGameRoot : SingletonMono<HGameRoot>
{
    private float volumeMultiplier = 1f;
    private float mouseSensitive = 1f;  //记录一下，方便下次打开设置界面的时候获取到对应的值
    private bool openPause = false;
    public bool gameStart = false;
    public CatGamePlayerData playerData;
    
    public float VolumeMultiplier
    {
        get => volumeMultiplier;
        set
        {
            volumeMultiplier = value;
        }
    }
    
    public float MouseSensitive
    {
        get => mouseSensitive;
        set
        {
            mouseSensitive = value;
        }
    }
    
    public bool OpenPause
    {
        get => openPause;
        set
        {
            openPause = value;
        }
    }

    private void Update()
    {
        //todo:暂时现在这里监听玩家是否有按下ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openPause || !gameStart) return;
            openPause = true;
            OurGameFramework.UIManager.Instance.Open(OurGameFramework.UIType.PausePanelView);
        }
    }

    private void OnGUI()
    {
        //just test button
        // if (GUI.Button(new Rect(10, 10, 100, 50), "ChangeSlider1"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", 20f);
        // }
        //
        // if (GUI.Button(new Rect(10, 70, 100, 50), "ChangeSlider1_minus"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", -10f);
        // }
        //
        // if (GUI.Button(new Rect(10, 130, 100, 50), "ChangeSlider2"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_VALUE_CHANGE, "Slider2", 15f);
        // }
        //
        // if (GUI.Button(new Rect(10, 190, 100, 50), "ChangeSlider2_minus"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_VALUE_CHANGE, "Slider2", -5f);
        // }
        //
        // if (GUI.Button(new Rect(10, 250, 100, 50), "ChangeSlider1UpperBound"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider1", 20f);
        // }
        //
        // if (GUI.Button(new Rect(10, 310, 100, 50), "ChangeSlider2UpperBound"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider1", -10);
        // }
    }
}
