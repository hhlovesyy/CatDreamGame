using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class YCountDownUI : MonoBehaviour
{
    protected GameObject countDownUI;
    protected TMP_Text countDownText;
    protected Image countDownImage;
    public string addCountDownUIlink = "Test_CountDownPanel";
    private GameObject CountDownPanel;
    
    public float skillLastTime=300f;  //技能的持续时间
    public float flashTime = 0.05f;
    Coroutine countDownCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        GameObject countDownUIGO = Addressables.InstantiateAsync(addCountDownUIlink).WaitForCompletion();
        
        countDownUI = Instantiate(countDownUIGO, GameObject.Find("Canvas").transform);
        countDownUI.gameObject.SetActive(false);
        
        countDownText = countDownUI.GetComponentInChildren<TMP_Text>();
        countDownImage = countDownUI.transform.Find("skillIcon").GetComponent<Image>();
        SetCountDownTickUI(false);
    }
    
    //仅显示或关闭图标，未开始倒计时
    public void Showicon(bool isShow)
    {
        countDownUI.gameObject.SetActive(isShow);
    }
    public void BeginCountDown()
    {
        //开始之前 把上一次的删了，因此如果需要多个倒计时，就需要增加多个YCountDownUI组件
        EndCountDown();
        // SetCountDownTickUI(true);
        countDownCoroutine = StartCoroutine(BeginCountDownCoroutine());
    }
    public void EndCountDown()
    {
        //countDownUI.gameObject.SetActive(false);
        SetCountDownTickUI(false);
        if(countDownCoroutine!=null)StopCoroutine(countDownCoroutine);
    }
    //开始倒计时或关闭倒计时
    void SetCountDownTickUI(bool isShow)
    {
        if (isShow == true)
        {
            // 如果此时还没打开图标，则打开图标
            Showicon(true);
            countDownText.gameObject.SetActive(true);
            countDownImage.gameObject.SetActive(true);
        }
        else
        {
            countDownText.gameObject.SetActive(false);
            countDownImage.gameObject.SetActive(false);
        }
    }
    
    

    IEnumerator BeginCountDownCoroutine()
    {
        countDownImage.fillAmount = 1;
        //countDownUI.gameObject.SetActive(true);
        SetCountDownTickUI(true);
        
        int tickCount = (int)(skillLastTime / flashTime );
        
        for(int i = 0;i < tickCount;i++)
        {
            yield return new WaitForSeconds(flashTime );

            float remainTime = skillLastTime - i * flashTime ;
            countDownText.text = remainTime.ToString("F1");
            countDownImage.fillAmount = remainTime / skillLastTime;
            
            // Debug.Log(remainTime);
        }
        SetCountDownTickUI(false);
    }
    
   

    
}
