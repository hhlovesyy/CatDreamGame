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
    protected float itemWeight;
    protected CollisionResultType collisionShowType;
    protected CollisionResultType hitShowType;
    protected CollisionResultType hitGroundShowType;
    protected bool hasEff = false;
    protected string EffFloorResourceShortPath;
    protected string EffFloorResourcLongPath;
    protected Rigidbody rigidbody;
    protected bool canCollisionImpactSlider;
    
    //是否碰撞处理slider的逻辑，有cd，暂时定为3s
    protected float sliderCD = 3f;
    protected bool canSliderChange = true;

    private bool isInTrigger = false;
    protected bool hasSpecialInteraction = false;
    
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
            //enum
            collisionShowType = (CollisionResultType)Enum.Parse(typeof(CollisionResultType), item._ItemCollisionShow());
            hitShowType = (CollisionResultType)Enum.Parse(typeof(CollisionResultType), item._ItemHitShow());
            hitGroundShowType = (CollisionResultType)Enum.Parse(typeof(CollisionResultType), item._ItemHitGroundShow());
            
            EffFloorResourceShortPath = item._EffFloorResourceShort();
            EffFloorResourcLongPath = item._EffFloorResourceLong();
            hasSpecialInteraction = (item._hasSpecialInteraction() == 1);
            if(EffFloorResourceShortPath != "null" || EffFloorResourcLongPath != "null")
            {
                hasEff = true;
            }

            canCollisionImpactSlider = (item._CollisionImpactSlider() == 1) ;
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.GetComponentInChildren<Rigidbody>();
            }
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
    
    private void OnTriggerStay(Collider other)
    {
        if (!hasSpecialInteraction) return; //省性能
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!hasSpecialInteraction) return;  //省性能
        EndSpecialInteraction();
    }
    
    protected virtual void StartSpecialInteraction(Collider other)
    {
        //这个函数用来开始特殊的物品交互
    }

    protected virtual void EndSpecialInteraction()
    {
        
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
 
        if (hasSpecialInteraction)  //有特殊的物品交互
        {
            StartSpecialInteraction(other);
        }
    }

    protected virtual void SliderChange()
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
    }
    protected virtual void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        //这个函数用来应用物品的物理效果
    }
    
    //类似洒水 水花等特殊效果
    protected virtual void ApplySpecialVfxEffect(Vector3 BrokenPosition)
    {
        if (hasEff)
        {
            string EffResourcePath;
            //将position移动到距离最近的地面上
            //检测地面的位置，检测tag为Floor的物体，BrokenPosition.y往下移动0.5f的范围内检测是否有地面
            //如果有地面，将BrokenPosition.y设置为地面的y值
            //如果没有地面，BrokenPosition = new Vector3(BrokenPosition.x, BrokenPosition.y-0.5f, BrokenPosition.z);
            // 检测距离最近的地面
            
            
            RaycastHit hit;
            if (Physics.Raycast(BrokenPosition, Vector3.down, out hit, 1f, LayerMask.GetMask("Floor")))
            {
                // 如果检测到地面，使用地面的y坐标
                // BrokenPosition.y = hit.transform.position.y;
                BrokenPosition.y = hit.point.y;
                EffResourcePath = EffFloorResourcLongPath;
            }
            else
            {
                // 如果未检测到地面，稍微降低y值
                BrokenPosition.y -= 0.44f;
                EffResourcePath = EffFloorResourceShortPath;
                // EffResourcePath = EffFloorResourcLongPath;
            }
            
            
            // BrokenPosition = new Vector3(BrokenPosition.x, BrokenPosition.y-0.5f, BrokenPosition.z);
            
            //这个函数用来应用物品的特效效果
            ResourceManager.Instance.InstantiateAsync(EffResourcePath, (obj) =>
            {
                obj.transform.position = BrokenPosition;
                // fractureGO = Instantiate(fractureObject, BrokenPosition, transform.rotation, HLevelManager.Instance.levelParent);
            });
        }
    }
    public virtual void ApplyItemEffect(bool isChangeSlider, CollisionInfo info)
    {
        // if (isChangeSlider)
        // {
        //     // SliderChange();
        // }
        
        ApplyItemPhysicsEffect(info);
    }
}
