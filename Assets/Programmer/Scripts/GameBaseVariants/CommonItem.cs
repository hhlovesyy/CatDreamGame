using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CommonItem : CatGameBaseItem
{
    public bool itemStartOnGround = false; //是否开始的时候物体就在地上
    protected override void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        base.ApplyItemPhysicsEffect(info);
        CollisionSourceType collisionSourceType = info.collisionSourceType;
        if(collisionSourceType == CollisionSourceType.GROUND)
        {
            if (itemStartOnGround)  //开始在地上的物品，不会触发碰撞，第二次撞击地面才会判定碰撞
            {
                itemStartOnGround = false;
                return;
            }
            else if (hitGroundShowType == CollisionResultType.COMMONFORCE)
            {
                DoPush(info);
                DoSliderChange();
            }
        }
        else if (collisionSourceType == CollisionSourceType.BODY)
        {
            if (collisionShowType == CollisionResultType.COMMONFORCE)
            {
                DoPush(info);
                if (canCollisionImpactSlider)
                {
                    DoSliderChange();
                }
            }
        }
        else if(collisionSourceType == CollisionSourceType.PAW)
        {
            if (hitShowType == CollisionResultType.COMMONFORCE)
            {
                DoPush(info);
                DoSliderChange();
            }
        }
    }

    private void PlaySound(string soundAddressableLink)
    {
        HAudioManager.Instance.Play(soundAddressableLink, this.gameObject);
    }

    private void DoSliderChange()
    {
        //给SliderChange加了一个3s的cd
        if (!canSliderChange) return;
        canSliderChange = false;
        SliderChange(); //slider change
        DOVirtual.DelayedCall(sliderCD, () =>
        {
            canSliderChange = true;
        });
    }
    
    private void DoPush(CollisionInfo info)
    {
        Vector3 force = info.collisionForce;
        Vector3 position = info.collisionPoint;
        if (rigidbody)
        {
            rigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
        }
        string audioLink = pushAudioLink;
        if (audioLink != null && audioLink!= "null")
        {
            if(info.collisionSourceType != CollisionSourceType.BODY)  //body的情况无音效，不然效果不好
            {
                PlaySound(audioLink);
            }
            
        }
    }

}
