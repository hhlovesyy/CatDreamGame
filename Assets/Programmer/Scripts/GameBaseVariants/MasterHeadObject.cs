using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MasterHeadObject : CatGameBaseItem
{
    private AudioSource audioSource;
    private Coroutine masterSayCoroutine;
    private List<string> sayList = new List<string>();
    public GameObject canvas;
    public TMP_Text sayText;
    
    private void OnGameStatusChange(GameStatusEvent gameStatusEvent)
    {
        if(masterSayCoroutine!=null) StopCoroutine(masterSayCoroutine);
        Debug.Log("gameStatusChange!!");
        if (gameStatusEvent == GameStatusEvent.GAME_WIN)
        {
            audioSource.Stop(); //这一关赢了，停止播放打鼾声
        }
        else
        {
            //这一关输了,主人鼾声大作
            //音量翻倍
            audioSource.volume = 2;
            audioSource.spatialBlend = 0f;
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveEvent<GameStatusEvent>(GameEvent.CHANGE_GAME_STATUS.ToString(), OnGameStatusChange);
    }

    public override void SetItemBaseAttribute()
    {
        audioSource = GetComponent<AudioSource>();
        //add listener, 胜利的时候停止播放打鼾声
        EventManager.AddEvent<GameStatusEvent>(GameEvent.CHANGE_GAME_STATUS.ToString(), OnGameStatusChange);
        //锁所有的rigidbody物理//禁止头有任何的物理结算！
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.mass = 10000f;
        
        //设置canvas的event camera
        Canvas canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        
        foreach (var say in SD_LevelMasterSay.Class_Dic)
        {
            string key = say.Key;
            Class_LevelMasterSay value = say.Value;
            if (value._worldAppearLevel() == HLevelManager.Instance.CurLevel)
            {
                sayList.Add(value.sayContent);
            }
        }
        
        masterSayCoroutine = StartCoroutine(MasterSaySomething());
    }

    IEnumerator MasterSaySomething()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            //时不时说几句话
            string say = sayList[Random.Range(0, sayList.Count)];
            sayText.text = say;
            canvas.GetComponent<CanvasGroup>().DOFade(1f, 2f);
            yield return new WaitForSeconds(10f);
            canvas.GetComponent<CanvasGroup>().DOFade(0f, 2f);
            yield return new WaitForSeconds(5f);
            sayText.text = "";
        }
    }

    protected override void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        CollisionSourceType collisionSourceType = info.collisionSourceType;
        if (collisionSourceType == CollisionSourceType.PAW) //只有爪子有用
        { 
            canvas.GetComponent<CanvasGroup>().DOFade(0f, 0.1f); 
            sayText.text = "";
            StopCoroutine(masterSayCoroutine);
           //重新开启协程
           masterSayCoroutine = StartCoroutine(MasterSaySomething());
           //slider change 2
           EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
               SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", -2);
        }
    }
}
