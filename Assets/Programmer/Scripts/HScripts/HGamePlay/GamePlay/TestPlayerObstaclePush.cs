using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerObstaclePush : MonoBehaviour
{
    public float pushMultiplier = 0.5f; //just for test
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("Obstacle hit");
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
            {
                return;
            }

            Vector3 pushDir = hit.gameObject.transform.position - transform.position;
            pushDir.y = 0;
            pushDir = pushDir.normalized;
            body.AddForceAtPosition(pushDir * pushMultiplier, transform.position, ForceMode.Impulse);
        }
    }
}
