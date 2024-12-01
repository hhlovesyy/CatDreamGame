using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class YCatAttackEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PawColliderLeft.SetActive(false);
        PawColliderRight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public GameObject PawColliderLeft;
    public GameObject PawColliderRight;
    public void BeginBeatPawLeft()
    {
        PawColliderLeft.SetActive(true);
        Debug.Log("BeginBeatPawLeft");
    }
    public void EndBeatPawLeft()
    {
        PawColliderLeft.SetActive(false);
        Debug.Log("EndBeatPawLeft");
    }
    public void BeginBeatPawRight()
    {
        PawColliderRight.SetActive(true);
        Debug.Log("BeginBeatPawRight");
    }
    public void EndBeatPawRight()
    {
        PawColliderRight.SetActive(false);
        Debug.Log("EndBeatPawRight");
    }
    
    public void BeginBeatPaw(bool isRight)
    {
        if (isRight)
        {
            PawColliderRight.SetActive(true);
            Debug.Log("BeginBeatPaw");
        }
        else
        {
            PawColliderLeft.SetActive(true);
            Debug.Log("BeginBeatPaw");
        }
    }
    
    public void EndBeatPaw(bool isRight)
    {
        if (isRight)
        {
            PawColliderRight.SetActive(false);
            Debug.Log("EndBeatPaw");
        }
        else
        {
            PawColliderLeft.SetActive(false);
            Debug.Log("EndBeatPaw");
        }
    }
    
}
