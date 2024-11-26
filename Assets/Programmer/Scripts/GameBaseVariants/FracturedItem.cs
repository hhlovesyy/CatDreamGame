using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FracturedItem : CatGameBaseItem
{
    protected override void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        base.ApplyItemPhysicsEffect(info);
        CollisionSourceType collisionSourceType = info.collisionSourceType;
        if(collisionSourceType == CollisionSourceType.GROUND)
        {
            if(hitGroundShowType == CollisionResultType.BROKEN)
            {
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
        ObjectFracture objectFracture = GetComponentInChildren<ObjectFracture>();
        if (objectFracture != null)
        {
            objectFracture.OnTriggerBroken();
        }

        
        ApplySpecialVfxEffect(objectFracture.transform.position);
        SliderChange();
    }
}
