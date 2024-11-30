using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PianoKeyDown : MonoBehaviour
{
    private Vector3 originPos;
    public Material pressMaterial;
    private Material originalMaterial;
    public int audioIndex;
    public float audioStartSecond;
    public float audioEndSecond;
    private bool canPlay = true;
    
    private void Start()
    {
        originPos = transform.position;
        originalMaterial = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (!canPlay) return;
            canPlay = false;
            DOVirtual.DelayedCall(0.2f, () =>
            {
                canPlay = true;
            });
            // PianoObject pianoObject = GetComponentInParent<PianoObject>();
            // pianoObject.PianoPlaying(true);
            //模拟按下琴键
            transform.DOMoveY(transform.position.y - 0.023f, 0.1f);
            GetComponent<MeshRenderer>().material = pressMaterial;
            //播放
            string audioLink = "PianoMusic" + audioIndex;
            HAudioManager.Instance.Play(audioLink, this.gameObject, audioStartSecond, audioEndSecond);
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider1", -1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //模拟松开琴键
            transform.DOMoveY(originPos.y, 0.1f);
            GetComponent<MeshRenderer>().material = originalMaterial;
            HAudioManager.Instance.Stop(this.gameObject);
        }
    }
}
