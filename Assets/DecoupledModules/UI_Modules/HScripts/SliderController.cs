using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using OurGameFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    //用于外部对Slider条做控制
    private Dictionary<string, Slider> sliderDict = new Dictionary<string, Slider>();
    private Dictionary<string, float> sliderCurValueDict = new Dictionary<string, float>();
    private Dictionary<string, float> sliderMaxValueDict = new Dictionary<string, float>();  //slider条的最大值，应由关卡策划表初始进行配置
    private int levelID = -1;
    private float sliderValueUpSpeed = 0.5f; //slider条值上升的速度
    private Dictionary<string, bool> sliderValueChangeDict = new Dictionary<string, bool>(); //用于记录slider条的值是否需要缓慢下降
    private bool isWin = false;

    private string sliderHandle1Link = "HandleStage1";
    private string sliderHandle2Link = "HandleStage2";
    private string sliderHandle3Link = "HandleStage3";
    private Sprite sliderHandle1;
    private Sprite sliderHandle2;
    private Sprite sliderHandle3;

    //255 242 90 -> float 1 0.949 0.353
    private Color MiddleLerpColor = new Color(1 ,0.949f, 0.353f);

    private void Awake()
    {
        //读Slider的Handle的图片，暂存起来
        sliderHandle1 = Addressables.LoadAssetAsync<Sprite>(sliderHandle1Link).WaitForCompletion();
        sliderHandle2 = Addressables.LoadAssetAsync<Sprite>(sliderHandle2Link).WaitForCompletion();
        sliderHandle3 = Addressables.LoadAssetAsync<Sprite>(sliderHandle3Link).WaitForCompletion();
    }

    private void SetHandleImage(string sliderName, Image handle)
    {
        //依据slider的值的比例决定handle的图片， 100%到50%是1，50%到0%是2，0%到-5%是3
        float value = sliderCurValueDict[sliderName];
        float maxValue = sliderMaxValueDict[sliderName];
        if (value >= maxValue * 0.5f)
        {
            handle.sprite = sliderHandle1;
        }
        else if (value >= 0)
        {
            handle.sprite = sliderHandle2;
        }
        else
        {
            handle.sprite = sliderHandle3;
        }
    }

    private void SetSliderValue(string sliderName, float value) //直接设置某个slider条的值
    {
        if (sliderDict.ContainsKey(sliderName))
        {
            sliderDict[sliderName].value = value;
            sliderCurValueDict[sliderName] = value;
            Transform handle = sliderDict[sliderName].transform.Find("Handle");
            SetHandleImage(sliderName, handle.GetComponent<Image>());
        }
    }
    
    private void DoSetSliderValue(string sliderName, float value) //通过DoTween设置某个slider条的值
    {
        if (sliderDict.ContainsKey(sliderName))
        {
            sliderDict[sliderName].DOValue(value, 0.5f);
            sliderCurValueDict[sliderName] = value;
            Transform handle = sliderDict[sliderName].transform.Find("Handle");
            SetHandleImage(sliderName, handle.GetComponent<Image>());
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
            sliderCurValueDict.Add(sliderName, maxValue);
            sliderValueChangeDict.Add(sliderName, true); 
            sliders[i].maxValue = maxValue;
            sliders[i].value = maxValue;
        }

        sliderValueUpSpeed = SD_SliderValueConfig.Class_Dic[levelID.ToString()]._SliderResumeSpeed();

    }
    
    public void SetSliders(List<Slider> sliders, int levelID) //设置slider条，在打开panel的时候进行设置
    {
        this.levelID = levelID;
        SetSliderBaseValues(levelID, sliders);
        StartCoroutine(SliderValueChanged());
    }
    
    IEnumerator SliderValueChanged()
    {
        while (!isWin)
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
                    float newValue = currentValue + sliderValueUpSpeed;
                    newValue = Mathf.Clamp(newValue, -5, maxValue);
                    sliderCurValueDict[slider.Key] = newValue;
                    SetSliderValue(slider.Key, newValue);
                }
            }

            CheckIsWin();
            yield return new WaitForSeconds(1f);
        }
    }
    
    public void ClearSliders() //清空slider条，在关闭panel的时候进行清空
    {
        sliderDict.Clear();
        sliderCurValueDict.Clear();
        sliderMaxValueDict.Clear();
        sliderValueChangeDict.Clear();
        isWin = false;
    }

    public void ChangeSliderValue(string name, float addValue, bool useDoTween) //动态调整slider的值
    {
        if (sliderDict.ContainsKey(name))
        {
            float currentValue = sliderCurValueDict[name];
            float maxValue = sliderMaxValueDict[name];
            float newValue = currentValue + addValue;
            newValue = Mathf.Clamp(newValue, -5, maxValue);  //trick, -5是为了宽判胜利条件，不然如果严格必须某个值的话很难获胜
            
            //todo:以下部分为了增加视觉效果，写的不太好，后面有时间再优化吧
            Slider slider = sliderDict[name];
            sliderValueChangeDict[name] = false; //此时不进行自动下降操作
            DOVirtual.DelayedCall(0.5f, () =>
            {
                sliderValueChangeDict[name] = true;
            });
            Image fillImage = slider.transform.Find("FillMiddle").GetComponent<Image>();
            fillImage.gameObject.SetActive(true);
            
            fillImage.color = newValue <= currentValue ? MiddleLerpColor : Color.red;
            fillImage.DOFillAmount(newValue / maxValue, 0.7f);
            DOVirtual.DelayedCall(0.7f, () =>
            {
                fillImage.gameObject.SetActive(false);
            });
            
            if (useDoTween && newValue > currentValue) //现在是上升，用dotween做
            {
                DoSetSliderValue(name, newValue);
            }
            else //其实tween的是fillImage，所以这里不用了
            {
                SetSliderValue(name, newValue);
            }
        }
    }

    public void ChangeSliderUpperBound(string name, float addUpperBound)
    {
        //addUpperBound 指的是对上限值的增加和减少
        if (sliderDict.ContainsKey(name))
        {
            //暂时先不考虑视觉效果
            float oldMaxValue = sliderMaxValueDict[name];
            float maxValue = oldMaxValue + addUpperBound;
            float sizeChangeDelta = maxValue / oldMaxValue;
            Vector2 curSizeDelta = sliderDict[name].GetComponent<RectTransform>().sizeDelta;
            
            maxValue = Mathf.Max(0, maxValue);
            sliderMaxValueDict[name] = maxValue;
            sliderValueChangeDict[name] = false; //此时不进行自动上升操作
            DOVirtual.DelayedCall(0.5f, () =>
            {
                sliderValueChangeDict[name] = true;  //0.5秒后再进行自动上升
            });
            
            //实时更新slider的值
            float curValue = sliderCurValueDict[name]; 
            sliderDict[name].maxValue = maxValue;
            sliderDict[name].value = curValue;
            //实时更新image的值
            Image fillImage = sliderDict[name].transform.Find("FillMiddle").GetComponent<Image>();
            fillImage.fillAmount = curValue / maxValue;
            
            //用dotween实时更新slider的大小
            sliderDict[name].GetComponent<RectTransform>()
                .DOSizeDelta(new Vector2(curSizeDelta.x, curSizeDelta.y * sizeChangeDelta), 1f);
        }
    }

    private void CheckIsWin()
    {
        if (isWin) return;
        foreach (var slider in sliderDict)
        {
            if (sliderCurValueDict[slider.Key] > 0)
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
        EventManager.DispatchEvent<GameStatusEvent>(GameEvent.CHANGE_GAME_STATUS.ToString(), GameStatusEvent.GAME_WIN);
    }
    

    private void Update()
    {
    }
}
