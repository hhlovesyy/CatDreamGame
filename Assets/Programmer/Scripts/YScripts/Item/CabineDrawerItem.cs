using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CabineDrawerItem : CatGameBaseItem
{
    
    protected override void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        base.ApplyItemPhysicsEffect(info);
        CollisionSourceType collisionSourceType = info.collisionSourceType;
        if (collisionSourceType == CollisionSourceType.GROUND) //撞到地上
        {
            return;
        }
        else if (collisionSourceType == CollisionSourceType.BODY)
        {
            DoPush(info);
            DoSliderChange();
            // if (collisionShowType == CollisionResultType.BROKEN)
            // {
            //     DoBroken();
            // }
            // else if (collisionShowType == CollisionResultType.COMMONFORCE)
            // {
            //     DoPush(info);
            //     if (canCollisionImpactSlider)
            //     {
            //         DoSliderChange();
            //     }
            // }
        }
        else if (collisionSourceType == CollisionSourceType.PAW)
        {
            DoPush(info);
            DoSliderChange();
        }

    }
    private void DoSliderChange()
    {
        //给SliderChange加了一个3s的cd
        if (!canSliderChange) return;
        canSliderChange = false;
        SliderChange(); //slider change
        DOVirtual.DelayedCall(sliderCD, () => { canSliderChange = true; });
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

   

}
