using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoObject : MonoBehaviour
{
    public List<Transform> pianoKeys;
    
    public void PianoPlaying(bool isPlaying)
    {
        //开始弹琴
        foreach(Transform key in pianoKeys)
        {
            key.gameObject.GetComponent<PianoKeyDown>().enabled = isPlaying;
            if (!isPlaying)
            {
                HAudioManager.Instance.Stop(key.gameObject);
            }
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        //开始弹琴
        PianoPlaying(true);
    }

    protected void OnTriggerExit(Collider other)
    {
        //结束弹琴
        if (!other.gameObject.CompareTag("Player")) return;
        PianoPlaying(false);
    }
}
