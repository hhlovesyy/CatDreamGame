using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FracturedItem : CatGameBaseItem
{
    private bool isBroken = false;
    protected override void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        base.ApplyItemPhysicsEffect(info);
        CollisionSourceType collisionSourceType = info.collisionSourceType;
        if(collisionSourceType == CollisionSourceType.GROUND)
        {
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
        if (isBroken) return;
        isBroken = true;
        ObjectFracture objectFracture = GetComponentInChildren<ObjectFracture>();
        if (objectFracture != null)
        {
            objectFracture.OnTriggerBroken();
        }

        if (objectFracture!=null)
        {
            ApplyBrokenVfxEffect(objectFracture.transform.position);
            ApplySpecialVfxEffect(objectFracture.transform.position);
        }
        else
        {
            Vector3 brokenPosition = transform.position;
            ApplyBrokenVfxEffect(brokenPosition);
            ApplySpecialVfxEffect(brokenPosition);
        }
        
        SliderChange();
    }
}
