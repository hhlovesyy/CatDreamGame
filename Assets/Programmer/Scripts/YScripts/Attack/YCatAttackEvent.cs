using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YCatAttackEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PawCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public GameObject PawCollider;
    public void BeginBeatPaw()
    {
        PawCollider.SetActive(true);
        Debug.Log("BeginBeatPaw");
    }
    public void EndBeatPaw()
    {
        PawCollider.SetActive(false);
        Debug.Log("EndBeatPaw");
    }
}
