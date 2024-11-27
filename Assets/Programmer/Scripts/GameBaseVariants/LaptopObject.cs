using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaptopObject : CatGameBaseItem
{
    private Coroutine continousPressCoroutine;
    private bool canInteractive = true;
    public Material genshinMat;
    public Material normalMat;
    public GameObject beingEffectObject;
    
    private void StartGenshin()
    {
        canInteractive = false; //启动中，不能再次交互
        DOVirtual.DelayedCall(10f, () =>
        {
            canInteractive = true;  //10s后才可以再次交互
        });
        //material be genshin
        MeshRenderer meshRenderer = beingEffectObject.GetComponent<MeshRenderer>();
        meshRenderer.material = genshinMat;
        genshinMat.SetFloat("_IsError", 0);
        
        //5s之后，报错，然后开启tween，每秒1伤害
        DOVirtual.DelayedCall(5f, () =>
        {
            genshinMat.SetFloat("_IsError", 1);
            //每秒钟1伤害, 一直进行下去
            continousPressCoroutine = StartCoroutine(SpecialSliderChange(-1));
        });
    }

    IEnumerator SpecialSliderChange(int value)
    {
        //1伤害值
        while (true)
        {
            EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
                SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", value);
            Debug.Log("1 damage");
            yield return new WaitForSeconds(1f);
        }
        
    }
    protected override void StartSpecialInteraction(Collider other)  //判断开始特殊交互
    {
        base.StartSpecialInteraction(other);
        
        if (!canInteractive || !other.gameObject.CompareTag("Player")) return;
        if(continousPressCoroutine != null)
        {
            StopCoroutine(continousPressCoroutine);
        }
        StartGenshin();
        
    }
    
    protected override void EndSpecialInteraction()
    {
        base.EndSpecialInteraction();
        if (!canInteractive) return;
        if(continousPressCoroutine != null)
        {
            StopCoroutine(continousPressCoroutine);
        }

        canInteractive = false;
        DOVirtual.DelayedCall(10f, () =>
        {
            canInteractive = true;  //10s后才可以再次交互
        });
    }
}
