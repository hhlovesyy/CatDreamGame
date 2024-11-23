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
    private void OnGUI()
    {
        //just test button
        if (GUI.Button(new Rect(10, 10, 100, 50), "ChangeSlider1"))
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", 20f);
        }

        if (GUI.Button(new Rect(10, 70, 100, 50), "ChangeSlider1_minus"))
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", -10f);
        }

        if (GUI.Button(new Rect(10, 130, 100, 50), "ChangeSlider2"))
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_VALUE_CHANGE, "Slider2", 15f);
        }

        if (GUI.Button(new Rect(10, 190, 100, 50), "ChangeSlider2_minus"))
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_VALUE_CHANGE, "Slider2", -5f);
        }

        if (GUI.Button(new Rect(10, 250, 100, 50), "ChangeSlider1UpperBound"))
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider1", 20f);
        }

        if (GUI.Button(new Rect(10, 310, 100, 50), "ChangeSlider2UpperBound"))
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider1", -10);
        }
    }
}
