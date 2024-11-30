using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HMicrowaveObject : CatGameBaseItem
{
    public Material brokenMat;
    private bool isMatChanged = false;
    private bool isBroken = false;
    private int brokenTimeCount = 0;
    private Coroutine microWaveCoroutine;
    public GameObject beingEffectObject;
    private bool isHitGround = false;
    private bool isInBrokenCD = false;
    protected override void ApplyItemPhysicsEffect(CollisionInfo info)
    {
        base.ApplyItemPhysicsEffect(info);
        CollisionSourceType collisionSourceType = info.collisionSourceType;
        if(collisionSourceType == CollisionSourceType.GROUND)
        {
            if (hitGroundShowType == CollisionResultType.BROKEN)
            {
                //掉地上再触发一次音效
                if (isHitGround) return;
                isHitGround = true;
                HAudioManager.Instance.Play("HeavyWoodGroundAudio", this.gameObject);
                SliderChange();
            }
        }
        else if (collisionSourceType == CollisionSourceType.BODY)
        {
           if (collisionShowType == CollisionResultType.COMMONFORCE) //身体碰到正常push就行
           {
                DoPush(info);
           }
        }
        else if(collisionSourceType == CollisionSourceType.PAW)
        {
            if(hitShowType == CollisionResultType.BROKEN)
            {
                DoBroken();
            }
            else if (hitShowType == CollisionResultType.COMMONFORCE)
            {
                DoPush(info);
            }

            if (isBroken) //已经碎了，可以正常推动
            {
                DoPush(info);
            }
        }
    }
    
    IEnumerator SpecialSliderChange(int value)
    {
        //2伤害值
        while (true)
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", value);
            yield return new WaitForSeconds(1f);
        }
    }

    private void DoBroken()
    {
        if (isBroken) return;
        if (isInBrokenCD) return;
        isInBrokenCD = true;
        DOVirtual.DelayedCall(2f, () =>
        {
            isInBrokenCD = false;
        });
        SliderChange();
        brokenTimeCount += 1;
        if (brokenTimeCount == 1) //第一次进broken
        {
            //开启微波炉的声音，每秒2伤害
            HAudioManager.Instance.Play("MicroWaveLaunchAudio", this.gameObject);
            microWaveCoroutine = StartCoroutine(SpecialSliderChange(-2));
        }
        if (brokenTimeCount == 2) //broken两次
        {
            HAudioManager.Instance.EaseOutAndStop(this.gameObject);
            StopCoroutine(microWaveCoroutine);
            microWaveCoroutine = null;
            isBroken = true;
            //material be broken
            MeshRenderer meshRenderer = beingEffectObject.GetComponent<MeshRenderer>();
            meshRenderer.material = brokenMat;
        }
    }
    
    private void DoPush(CollisionInfo info)
    {
        Vector3 force = info.collisionForce;
        Vector3 position = info.collisionPoint;
        if (rigidbody)
        {
            rigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
        }
        string audioLink = pushAudioLink;
        if (audioLink != null && audioLink!= "null")
        {
            if(info.collisionSourceType != CollisionSourceType.BODY)  //body的情况无音效，不然效果不好
            {
                HAudioManager.Instance.Play(audioLink, this.gameObject);
            }
            
        }
    }
}
