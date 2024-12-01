using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class EndGamePlayVideo : MonoBehaviour
{
    // Start is called before the first frame update

    void GoBackToMainMenu(VideoPlayer player)
    {
        OurGameFramework.UIManager.Instance.Open(OurGameFramework.UIType.GameWelcomePanel);
        HGameRoot.Instance.OpenPause = false;
        EventSystem.current.SetSelectedGameObject(null);
        HLevelManager.Instance.ClearAllLevels();
        //关闭这个组件
        Destroy(this, 2f);
    }
    void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            var videoPlayer = mainCamera.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
            videoPlayer.targetCameraAlpha = 1F;
            videoPlayer.url = "Assets/Videos/EndPV/EndPV.mp4";
            videoPlayer.frame = 100;
            videoPlayer.isLooping = false;
            //结束回调函数
            videoPlayer.loopPointReached += GoBackToMainMenu;
            videoPlayer.Play();
        }
        
    }
    
}
