using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerObstaclePush : MonoBehaviour
{
    private float pushMultiplier = 0.5f;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            // //Debug.Log("Obstacle hit");
            // Rigidbody body = hit.collider.attachedRigidbody;
            // if (body == null || body.isKinematic)
            // {
            //     return;
            // }
            
            CollisionInfo info = new CollisionInfo();
            Vector3 pushDir = hit.gameObject.transform.position - transform.position;
            pushDir.y = 0;
            pushDir = pushDir.normalized;
            info.collisionForce = pushDir * pushMultiplier;
            info.collisionPoint = hit.point;
            info.collisionSourceType = CollisionSourceType.BODY;
            CatGameBaseItem item = hit.gameObject.GetComponentInParent<CatGameBaseItem>();
            if (item)
            {
                item.ApplyItemEffect(true, info);
            }
        }
    }
}
