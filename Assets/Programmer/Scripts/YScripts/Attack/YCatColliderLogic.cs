using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YCatColliderLogic : MonoBehaviour
{
    public float pushMultiplier = 0.5f; //just for test
    private void OnCollisionEnter(Collision hit)
    {
        Debug.Log("OnCollisionEnter");
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("Obstacle hit");
            CatGameBaseItem item = hit.gameObject.GetComponentInParent<CatGameBaseItem>();
            CollisionInfo info = new CollisionInfo();
            Vector3 pushDir = hit.gameObject.transform.position - transform.position;
            pushDir.y = 0;
            pushDir = pushDir.normalized;
            //body.AddForceAtPosition(pushDir * pushMultiplier, transform.position, ForceMode.Impulse);
            info.collisionForce = pushDir * pushMultiplier;
            info.collisionSourceType = CollisionSourceType.PAW;
            info.collisionPoint = hit.contacts[0].point;
            if (item)
            {
                item.ApplyItemEffect(true, info);
            }
            //碰到即碎
            // ObjectFracture objectFracture = hit.gameObject.GetComponent<ObjectFracture>();
            // if (objectFracture != null)
            // {
            //     objectFracture.OnTriggerBroken();
            // }
        }
        
        
    }

}
