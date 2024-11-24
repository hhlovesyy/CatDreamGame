using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using OurGameFramework;
using UnityEngine;

public class CatGameBaseItem : MonoBehaviour
{
    public int itemID = -1;
    private string itemName;
    private string itemDescription;
    private string itemSliderEffect;
    private string itemSliderUpperBoundEffect;
    private int itemWeight;

    private bool isInTrigger = false;
    
    private void Start()
    {
        if (itemID == -1)
        {
            Debug.LogError("itemID is not set!!");
        }
        Class_CatgameCommonItem item = SD_CatgameCommonItem.Class_Dic[itemID.ToString()];
        if (item != null)
        {
            itemName = item._ItemName();
            itemDescription = item._ItemDescription();
            itemSliderEffect = item._ItemSliderEffect();
            itemSliderUpperBoundEffect = item._ItemSliderUpperBoundEffect();
            itemWeight = item._ItemWeight();
        }

        SetItemBaseAttribute();
    }

    public virtual void SetItemBaseAttribute()
    {
        //暂时只包括物体基本属性，目前就一个物理属性
        Rigidbody rb = gameObject.GetComponentInChildren<Rigidbody>();
        if (!rb)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.mass = itemWeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        //这个函数暂时用来显示物品的描述
        if (other.gameObject.CompareTag("Player"))
        {
            if (isInTrigger) return;
            isInTrigger = true;
            // Debug.Log("===================================");
            // Debug.Log("Item Slider Effect: " + itemSliderEffect);
            // Debug.Log("Item Slider Upper Bound Effect: " + itemSliderUpperBoundEffect);
            // Debug.Log("===================================");
            string message = "item slider effect: " + itemSliderEffect + "itemWeight: " + itemWeight;
            HMessageShowMgr.Instance.ShowMessage("LEVEL_IN_MSG_0", message);
            DOVirtual.DelayedCall(3f, () =>
            {
                isInTrigger = false;
            });
        }
    }

    public virtual void ApplyItemEffect()
    {
        //这个函数用来应用物品的效果
        Debug.Log("Applying item effect...");
        //split ;
        string[] sliderEffects = itemSliderEffect.Split(';');
        string[] upperBoundEffects = itemSliderUpperBoundEffect.Split(';');
        for (int i = 0; i < sliderEffects.Length; i++)
        {
            if (upperBoundEffects[i] != "null")
            {
                float upperBoundEffectValue = float.Parse(upperBoundEffects[i]);
                EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                    SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider"+ (i + 1), upperBoundEffectValue);
            }
            
            if (sliderEffects[i] != "null")
            {
                float sliderEffectValue = float.Parse(sliderEffects[i]);
                EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                    SliderEvent.SLIDER_VALUE_CHANGE, "Slider"+ (i + 1), sliderEffectValue);
            }
        }
        Destroy(gameObject);
    }
}
