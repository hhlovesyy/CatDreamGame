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
            ObjectFracture objectFracture = other.gameObject.GetComponent<ObjectFracture>();
            if (objectFracture != null)
            {
                objectFracture.OnTriggerBroken();
            }
        }
    }
}
