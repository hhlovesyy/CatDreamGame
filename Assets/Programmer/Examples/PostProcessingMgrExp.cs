using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PostProcessingMgrExp : MonoBehaviour
{
    private bool hasSSAO = false;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 200), "测试后处理效果"))
        {
            //HPostProcessingManager.Instance.SetPostProcessingWithNameAndTime("xiaojingxi", 2f);
            //HPostProcessingManager.Instance.SetPostProcessingWithNameAndTime("Sexiangpianyi", 2f);
            HPostProcessingManager.Instance.SetPostProcessingWithNameAndTime("SexiangpianyiMove", 4f);
        }
        if(GUI.Button(new Rect(10, 230, 300, 200), "测试RenderFeature开/关"))
        {
            ScriptableRendererFeature feature = HPostProcessingManager.Instance.GetRenderFeature("SSAO");
            if (feature)
            {
                feature.SetActive(hasSSAO);
                hasSSAO = !hasSSAO;
            }
        }
        
    }
}
