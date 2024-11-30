using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRotateToMainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //billboard
        transform.LookAt(Camera.main.transform, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
