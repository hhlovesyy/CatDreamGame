using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFractureSuitScale : ObjectFracture 
{
    // public GameObject originalObject;  //原始物体，是挂载的预制体
    // public GameObject fractureObject;  //碎片物体，是挂载的预制体
    // public float minExplosionForce, maxExplosionForce;
    //
    // public float mExplosionRadius;
    private GameObject fractureGO; //碎片物体的实例,会通过代码生成上去
    Vector3 vfxV3 = new Vector3(0, 0, 0);
    Vector3 localScale = new Vector3(1f, 1f, 1f);
    private void Start()
    {
        vfxV3 = new Vector3(originalObject.transform.position.x, originalObject.transform.position.y,
            originalObject.transform.position.z);
        //找到挂载CatGameBaseItem的物体或者父物体的localscale
        CatGameBaseItem item = originalObject.GetComponentInParent<CatGameBaseItem>();
        if (item == null)
        {
            item = originalObject.GetComponent<CatGameBaseItem>();
        }

        // if (item != null)
        // {
        //     localScale = item.transform.localScale;
        // }
    }
    
    public virtual void OnTriggerBroken()
    {
        if (originalObject != null)
        {
            originalObject.SetActive(false);
        }

        if (fractureObject != null) //摔碎的逻辑
        {
            //挂载在父节点下面，父节点下有origin。origin寄了后挂载broken
            Vector3 BrokenPosition = originalObject.transform.position;
            Transform parent;
            parent = originalObject.transform.parent.transform;
            
            fractureGO = Instantiate(fractureObject, BrokenPosition, transform.rotation, parent);
            //!!!这里的localScale是原始origin物体的localScale
            //fractureGO.transform.localScale = originalObject.transform.localScale;
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

            //SendMessageToDownSlider();
            Destroy(originalObject, 0.1f);
        }
        
    }

    private void SendMessageToDownSlider()
    {
        // CatGameBaseItem item = originalObject.GetComponentInParent<CatGameBaseItem>();
        // if (item)
        // {
        //     item.ApplyItemEffect();
        // }
        // Destroy(item.gameObject);
    }
    
}
