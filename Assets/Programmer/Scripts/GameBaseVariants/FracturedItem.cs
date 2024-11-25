using System.Collections;
using System.Collections.Generic;
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
                SliderChange();
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
                SliderChange();
            }
        }
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
