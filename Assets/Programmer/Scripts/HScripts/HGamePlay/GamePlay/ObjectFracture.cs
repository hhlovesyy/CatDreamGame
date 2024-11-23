using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFracture : MonoBehaviour
{
    public GameObject originalObject;  //原始物体，是挂载的预制体
    public GameObject fractureObject;  //碎片物体，是挂载的预制体
    public float minExplosionForce, maxExplosionForce;

    public float mExplosionRadius;
    
    private GameObject fractureGO; //碎片物体的实例,会通过代码生成上去
    Vector3 vfxV3 = new Vector3(0, 0, 0);
    private void Start()
    {
        vfxV3 = new Vector3(originalObject.transform.position.x, originalObject.transform.position.y,
            originalObject.transform.position.z);
    }
    
    public void OnTriggerBroken()
    {
        if (originalObject != null)
        {
            originalObject.SetActive(false);
        }

        if (fractureObject != null) //摔碎的逻辑
        {
            fractureGO = Instantiate(fractureObject, vfxV3, transform.rotation);
            foreach (Transform tPiece in fractureGO.transform)
            {
                Rigidbody rb = tPiece.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    //让他们崩开
                    // rb.AddExplosionForce(Random.Range(minExplosionForce, maxExplosionForce), vfxV3, mExplosionRadius);
                    rb.AddExplosionForce(UnityEngine.Random.Range(minExplosionForce, maxExplosionForce), transform.position, mExplosionRadius);
                }
                //慢慢变小小时
                //StartCoroutine(Shrink(tPiece));
            }

            SendMessageToDownSlider();
        }
        
    }

    private void SendMessageToDownSlider()
    {
        //todo:依据策划表会改这个对应的值，但大体这么分发事件
        EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(), SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", -10f);
    }
    
}
