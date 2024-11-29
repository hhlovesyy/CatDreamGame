using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoObject : CatGameBaseItem
{
    public List<Transform> pianoKeys;
    
    public void PianoPlaying(bool isPlaying)
    {
        //开始弹琴
        
    }
    
    
    protected override void StartSpecialInteraction(Collider other) //判断开始特殊交互
    {
        base.StartSpecialInteraction(other);
        if (!other.gameObject.CompareTag("Player")) return;
        //开始弹琴
        PianoPlaying(true);
    }

    protected override void EndSpecialInteraction()
    {
        base.EndSpecialInteraction();
        //结束弹琴
        PianoPlaying(false);
    }
}
