using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTriggerBroken : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            CatGameBaseItem item = other.gameObject.GetComponentInParent<CatGameBaseItem>();
            if (item)
            {
                CollisionInfo info = new CollisionInfo();
                info.collisionSourceType = CollisionSourceType.GROUND;
                info.collisionVelocity = other.relativeVelocity;
                // info.collisionPoint = other.contacts[0].point;
                // info.collisionForce = other.impulse;
                item.ApplyItemEffect(true, info);
            }
            // ObjectFracture objectFracture = other.gameObject.GetComponent<ObjectFracture>();
            // if (objectFracture != null)
            // {
            //     objectFracture.OnTriggerBroken();
            // }
        }
    }
}
