using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    //用于外部对Slider条做控制
    private Dictionary<string, Slider> sliderDict = new Dictionary<string, Slider>();
    private Dictionary<string, float> sliderCurValueDict = new Dictionary<string, float>();
    private Dictionary<string, float> sliderMaxValueDict = new Dictionary<string, float>();  //slider条的最大值，应由关卡策划表初始进行配置
    private int levelID = -1;
    private float sliderValueDownSpeed = 0.5f; //slider条值下降的速度
    private Dictionary<string, bool> sliderValueChangeDict = new Dictionary<string, bool>(); //用于记录slider条的值是否需要缓慢下降
    private bool isWin = false;
    
    private void SetSliderValue(string sliderName, float value) //直接设置某个slider条的值
    {
        if (sliderDict.ContainsKey(sliderName))
        {
            sliderDict[sliderName].value = value;
            sliderCurValueDict[sliderName] = value;
        }
    }
    
    private void DoSetSliderValue(string sliderName, float value) //通过DoTween设置某个slider条的值
    {
        if (sliderDict.ContainsKey(sliderName))
        {
            sliderDict[sliderName].DOValue(value, 0.5f);
            sliderCurValueDict[sliderName] = value;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                sliderValueChangeDict[sliderName] = true;
            });
        }
    }

    private void SetSliderBaseValues(int levelID, List<Slider> sliders)
    {
        //从策划表中拿这一关的数据
        string maxValueConfig = SD_SliderValueConfig.Class_Dic[levelID.ToString()]._SliderRequireValue();
        string[] maxValueArray = maxValueConfig.Split(';');

        string sliderNameConfig = SD_SliderValueConfig.Class_Dic[levelID.ToString()]._SliderNames();
        string[] sliderNameArray = sliderNameConfig.Split(';');
        
        for (int i = 0; i < sliderNameArray.Length; i++) //初始化值的操作
        {
            string sliderName = sliderNameArray[i];
            float maxValue = float.Parse(maxValueArray[i]);
            sliderMaxValueDict.Add(sliderName, maxValue);
            sliderDict.Add(sliderName, sliders[i]);
            sliderCurValueDict.Add(sliderName, 0);
            sliderValueChangeDict.Add(sliderName, true); 
            sliders[i].maxValue = maxValue;
            sliders[i].value = 0;
        }

    }
    
    public void SetSliders(List<Slider> sliders, int levelID) //设置slider条，在打开panel的时候进行设置
    {
        this.levelID = levelID;
        SetSliderBaseValues(levelID, sliders);
    }
    
    public void ClearSliders() //清空slider条，在关闭panel的时候进行清空
    {
        sliderDict.Clear();
        sliderCurValueDict.Clear();
        sliderMaxValueDict.Clear();
    }

    public void ChangeSliderValue(string name, float addValue, bool useDoTween) //动态调整slider的值
    {
        if (sliderDict.ContainsKey(name))
        {
            float currentValue = sliderCurValueDict[name];
            float maxValue = sliderMaxValueDict[name];
            float newValue = currentValue + addValue;
            newValue = Mathf.Clamp(newValue, 0, maxValue + 5);  //trick, +5是为了宽判胜利条件，不然如果严格必须某个值的话很难获胜
            
            //todo:以下部分为了增加视觉效果，写的不太好，后面有时间再优化吧
            Slider slider = sliderDict[name];
            sliderValueChangeDict[name] = false; //此时不进行自动下降操作
            Image fillImage = slider.transform.Find("FillMiddle").GetComponent<Image>();
            fillImage.gameObject.SetActive(true);
            fillImage.color = newValue >= currentValue ? Color.green : Color.red;
            fillImage.DOFillAmount(newValue / maxValue, 0.2f);
            DOVirtual.DelayedCall(0.7f, () =>
            {
                fillImage.gameObject.SetActive(false);
            });
            
            if (useDoTween)
            {
                DoSetSliderValue(name, newValue);
            }
            else
            {
                SetSliderValue(name, newValue);
            }
        }
    }

    private void CheckIsWin()
    {
        foreach (var slider in sliderDict)
        {
            if (sliderCurValueDict[slider.Key] < sliderMaxValueDict[slider.Key])
            {
                isWin = false;
                return;
            }
        }
        isWin = true;
        ShowLevelWin();
    }

    private void ShowLevelWin()
    {
        Debug.Log("now you win this level!!");
    }

    private void Update()
    {
        if (sliderDict.Count > 0)
        {
            foreach (var slider in sliderDict)
            {
                if (!sliderValueChangeDict[slider.Key])  //如果不需要缓慢下降，就不进行下降操作
                {
                    continue;
                }
                float currentValue = sliderCurValueDict[slider.Key];
                float maxValue = sliderMaxValueDict[slider.Key];
                float newValue = currentValue - sliderValueDownSpeed * Time.deltaTime;
                newValue = Mathf.Clamp(newValue, 0, maxValue + 5);
                sliderCurValueDict[slider.Key] = newValue;
                SetSliderValue(slider.Key, newValue);
            }
        }

        CheckIsWin();
    }
}
