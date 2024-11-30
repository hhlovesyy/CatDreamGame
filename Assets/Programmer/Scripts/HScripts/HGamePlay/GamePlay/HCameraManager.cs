using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HCameraManager : SingletonMono<HCameraManager>
{
    private CinemachineBrain cinemachineBrain;
    
    float shakeTimer;//抖动时间
    float shakeTimerTotal;//抖动总时间
    float startingIntensity;//抖动强度
    
    CinemachineVirtualCameraBase cinemachineVirtualCameraBase;//virtualcamera 的基类 
    CinemachineBasicMultiChannelPerlin[] cinemachineBasicMultiChannelPerlins;//这里是获取所有的rig来设置noise
    void Start()
    {
        GameObject mainCamera = Camera.main.gameObject;
        if (mainCamera)
        {
            cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
        }
    }

    public void ShakeCamera(float intensity, float time, float frequency = 0.1f)
    {
        GameObject cinemachine = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject;
        if (cinemachine)
        {
            VirtualCinemachineShake cinemachineShake = cinemachine.GetComponent<VirtualCinemachineShake>();
            if (cinemachineShake)
            {
                cinemachineShake.ShakeCamera(intensity, time, frequency);
            }
        }
    }
    
    
    
}
