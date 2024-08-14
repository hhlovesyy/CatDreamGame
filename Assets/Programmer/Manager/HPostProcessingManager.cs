using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HPostProcessingManager : MonoBehaviour
{
    private static HPostProcessingManager _instance;
    public static HPostProcessingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HPostProcessingManager>();
            }
            return _instance;
        }
    }
    
    private Volume postProcessVolume;

    public Volume PostProcessVolume
    {
        get => postProcessVolume;
    }

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        PreloadRenderFeature();
    }
    
    public ScriptableRendererFeature unitRendererFeature;
    List<ScriptableRendererFeature> srfList;
    void PreloadRenderFeature()
    {
        ScriptableRendererFeature unitRendererFeature=null;
        UniversalRenderPipelineAsset _pipelineAssetCurrent = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;  // 通过GraphicsSettings获取当前的配置
        _pipelineAssetCurrent = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;  // 通过QualitySettings获取当前的配置
        //_pipelineAssetCurrent = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;  // 通过QualitySettings获取不同等级的配置

        // 也可以通过QualitySettings.names遍历所有配置

        srfList = _pipelineAssetCurrent.scriptableRenderer.GetType().
                GetProperty("rendererFeatures",
                    BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(_pipelineAssetCurrent.scriptableRenderer, null)
            as List<ScriptableRendererFeature>;

    }
    
    public void SetPostProcessingWithNameAndTime(string effect, float time)
    {
        StartCoroutine(SetPostProcessingEffect(effect, time));
    }
    
    IEnumerator SetPostProcessingEffect(string effect, float time)
    {
        float originValue = 0f;
        LensDistortion lensDistortion;
        ColorAdjustments colorAdjustments;
        ColorCurves colorCurves;
        
        switch (effect)
        {
            case "xiaojingxi":
                if(postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion))
                {
                    lensDistortion.intensity.value = 1f;
                    yield return new WaitForSeconds(time);
                    lensDistortion.intensity.value = originValue;
                }
                break;
            case "Sexiangpianyi":
                if (postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
                {
                    colorAdjustments.hueShift.value = 180f;
                    yield return new WaitForSeconds(time);
                    colorAdjustments.hueShift.value = originValue;
                }
                break;
            case "SexiangpianyiMove":
                if (postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
                {
                    var sequence = DOTween.Sequence();
                    //15s的时间，hueShift从0到180，再从180到-180，每0.1秒更新一次
                    sequence.Append(DOTween.To(() => colorAdjustments.hueShift.value, x => colorAdjustments.hueShift.value = x, 180f, time/4)); 
                    sequence.Append(DOTween.To(() => colorAdjustments.hueShift.value, x => colorAdjustments.hueShift.value = x, -180f, time/4));
                    sequence.Append(DOTween.To(() => colorAdjustments.hueShift.value, x => colorAdjustments.hueShift.value = x, 180f, time/4)); 
                    sequence.Append(DOTween.To(() => colorAdjustments.hueShift.value, x => colorAdjustments.hueShift.value = x, -180f, time/4));
                    yield return new WaitForSeconds(time);
                }
                colorAdjustments.hueShift.value = originValue;
                break;
            case "HeibaiHong":
                //开启我为逝者哀哭
                if (postProcessVolume.profile.TryGet<ColorCurves>(out colorCurves))
                {
                    colorCurves.active = true;
                    if (postProcessVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
                    {
                        var sequence = DOTween.Sequence();
                        //2s的时间把saturation从0到-100，过10s之后从-100重置回originValue
                        float duration = 2f * (1.0f / Time.timeScale);
                        sequence.Append(DOTween.To(() => colorAdjustments.saturation.value, x => colorAdjustments.saturation.value = x, -60f, duration));
                        yield return new WaitForSeconds(time);
                        colorAdjustments.saturation.value = originValue;
                        colorCurves.active = false;
                    }
                    
                }

                break;
                
        }
    }
    
    public ScriptableRendererFeature GetRenderFeature(string featureName)  //这里的参数是RenderFeature
    {
        // 创建一个字典用于存储特效名称和对应的特效对象
        // Dictionary<string, ScriptableRendererFeature> effsDict = new Dictionary<string, ScriptableRendererFeature>();

        foreach (ScriptableRendererFeature srf in srfList)
        {
            if (!string.IsNullOrEmpty(srf.name) && srf.name==featureName)
            {
                return srf;
            }
        }
        return null;
        /*
        使用的时候
        //test后处理unitRendererFeature
        unitRendererFeature = GetRenderFeature("FullScreenDoubleBonus");
        //开启这个特效
        if (unitRendererFeature != null)
        {
            unitRendererFeature.SetActive(true);
        }
        或者
        ScriptableRendererFeature renderFeature = HPostProcessingFilters.Instance.GetRenderFeature("FullScreenInvincible");
        renderFeature.SetActive(true);
        
         */
    }
    
    
}
