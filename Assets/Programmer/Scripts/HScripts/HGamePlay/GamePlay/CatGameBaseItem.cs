using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using OurGameFramework;
using UnityEngine;
using UnityEngine.InputSystem;

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
    protected bool hasBrokenEff = false;
    protected string EffBrokenVfxLinkPath;
    protected bool hasFloorEff = false;
    protected string EffFloorResourceShortPath;
    protected string EffFloorResourcLongPath;
    protected Rigidbody rigidbody;
    protected bool canCollisionImpactSlider;
    
    //是否碰撞处理slider的逻辑，有cd，暂时定为3s
    protected float sliderCD = 3f;
    protected bool canSliderChange = true;

    private bool isInTrigger = false;
    protected bool hasSpecialInteraction = false;
    
    //sound relative
    protected string pushAudioLink;
    protected string brokenAudioLink;
    protected string specialAudioLink;

    protected bool willShakingCamera = false;
    private CinemachineImpulseSource cinemachineImpulseSource;
    
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
                hasFloorEff = true;
            }
            EffBrokenVfxLinkPath = item._BrokenVfxLink();
            hasBrokenEff = EffBrokenVfxLinkPath != "null";

            canCollisionImpactSlider = (item._CollisionImpactSlider() == 1) ;
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.GetComponentInChildren<Rigidbody>();
            }

            pushAudioLink = item._pushAudio();
            brokenAudioLink = item._brokenAudio();
            specialAudioLink = item._pushBrokenSpecialAudio();
            willShakingCamera = (item._IsBrokenShakeCamera() == 1);
            if (willShakingCamera)
            {
                cinemachineImpulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
                cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseType =
                    CinemachineImpulseDefinition.ImpulseTypes.Dissipating;
                //explosoion
                cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseShape =
                    CinemachineImpulseDefinition.ImpulseShapes.Explosion;
                cinemachineImpulseSource.m_ImpulseDefinition.m_DissipationDistance = 20f;
                //dissiplation rate:0.25
                cinemachineImpulseSource.m_ImpulseDefinition.m_DissipationRate = 0.25f;
            }
        }

        SetItemBaseAttribute();
    }

    protected virtual void DoCameraShake(CollisionInfo info)
    {
        //基类：依据策划表中的信息决定震屏效果
        if (willShakingCamera)
        {
            //todo:暂时先全部震屏
            cinemachineImpulseSource.GenerateImpulseAtPositionWithVelocity(gameObject.transform.position, info.collisionVelocity);
            //Debug.Log("Shaking Camera...");
            DoGamePadShake();
        }
    }
    
    private void DoGamePadShake()
    {
        HGameRoot.Instance.GamepadVibrate(0.5f, 1f, 0.5f);
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
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     if (isInTrigger) return;
        //     isInTrigger = true;
        //     // Debug.Log("===================================");
        //     // Debug.Log("Item Slider Effect: " + itemSliderEffect);
        //     // Debug.Log("Item Slider Upper Bound Effect: " + itemSliderUpperBoundEffect);
        //     // Debug.Log("===================================");
        //     //string message = "item slider effect: " + itemSliderEffect + "itemWeight: " + itemWeight;
        //     // string message = itemName + "\n" + itemID;
        //     // HMessageShowMgr.Instance.ShowMessage("LEVEL_IN_MSG_0", message);
        //     // DOVirtual.DelayedCall(3f, () =>
        //     // {
        //     //     isInTrigger = false;
        //     // });
        // }
 
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
    protected virtual void ApplyBrokenVfxEffect(Vector3 BrokenPosition)
    {
        if (hasBrokenEff)
        {
            ResourceManager.Instance.InstantiateAsync(EffBrokenVfxLinkPath, (obj) =>
            {
                obj.transform.position = BrokenPosition;
            });
        }
    }
    protected virtual void ApplyBrokenVfxEffect(Vector3 BrokenPosition,Vector3 scale)
    {
        if (hasBrokenEff)
        {
            ResourceManager.Instance.InstantiateAsync(EffBrokenVfxLinkPath, (obj) =>
            {
                obj.transform.position = BrokenPosition;
                obj.transform.localScale = scale;
            });
        }
    }
    
    protected virtual void ApplyBrokenVfxEffect(Vector3 BrokenPosition, Transform parent)
    {
        if (hasBrokenEff)
        {
            ResourceManager.Instance.InstantiateAsync(EffBrokenVfxLinkPath, (obj) =>
            {
                obj.transform.position = BrokenPosition;
                obj.transform.SetParent(parent);
                obj.transform.localScale = new Vector3(1, 1, 1);
            });
        }
    }
    //类似洒水 水花等特殊效果
    protected virtual void ApplySpecialVfxEffect(Vector3 BrokenPosition)
    {
        if (hasFloorEff)
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
