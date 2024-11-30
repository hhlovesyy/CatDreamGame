using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonWithRope : MonoBehaviour
{
    //将这个气球的gravity设置为负数，让它向上飘
    Rigidbody rb;
    [SerializeField]float FlashTime = 0.5f;
    [SerializeField]float GoUpForce = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        //将这个气球的gravity设置为负数，让它向上飘
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        // rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        
        StartCoroutine(StartGoUp());
    }
    //每隔一段时间就让气球向上飘，使用dotween
    IEnumerator StartGoUp()
    {
        while (true)
        {
            rb.AddForce(Vector3.up * GoUpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(FlashTime);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
