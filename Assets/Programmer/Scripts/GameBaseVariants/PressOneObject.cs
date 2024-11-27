using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PressOneObject : CatGameBaseItem
{
    //这种对应砸地上或者被扒拉的时候会额外响一声，一共最多可以响两声
    private bool isBeenPressed = false;
    public float secondHurtValue = 1f; //第二次伤害的值
    protected override void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        base.ApplyItemPhysicsEffect(info);
        CollisionSourceType collisionSourceType = info.collisionSourceType;
        if(collisionSourceType == CollisionSourceType.GROUND)  //撞到地上
        {
            if (!isBeenPressed)
            {
                isBeenPressed = true;
                EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                    SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", secondHurtValue);
            }
            if(hitGroundShowType == CollisionResultType.BROKEN)
            {
                //if(info.collisionVelocity.magnitude > brokenThreshold)  //todo:后面可以加一个阈值，如果速度大于某个值，才会触发
                DoBroken();
            }
            else if (hitGroundShowType == CollisionResultType.COMMONFORCE)
            {
                DoPush(info);
                DoSliderChange();
            }
        }
        else if (collisionSourceType == CollisionSourceType.BODY)
        {
            if(collisionShowType == CollisionResultType.BROKEN)
            {
                DoBroken();
            }
            else if (collisionShowType == CollisionResultType.COMMONFORCE)
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
            if (!isBeenPressed)
            {
                isBeenPressed = true;
                EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                    SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", secondHurtValue);
            }
            if(hitShowType == CollisionResultType.BROKEN)
            {
                DoBroken();
            }
            else if (hitShowType == CollisionResultType.COMMONFORCE)
            {
                DoPush(info);
                DoSliderChange();
            }
        }
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
    }

    private void DoBroken()
    {
        ObjectFracture objectFracture = GetComponentInChildren<ObjectFracture>();
        if (objectFracture != null)
        {
            objectFracture.OnTriggerBroken();
        }

        
        ApplySpecialVfxEffect(objectFracture.transform.position);
        SliderChange();
    }
}
