using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OurGameFramework
{
    public class PausePanelView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
		#pragma warning disable 0649
		[ControlBinding]
		private Button ReturnGameBtn;
		[ControlBinding]
		private Button ReturnWelcomePanel;
		[ControlBinding]
		private Button SettingBtn;

		#pragma warning restore 0649
        #endregion

        private float scaleTime = 1f;
        private void ReturnToGame()
        {
            Time.timeScale = scaleTime;
            HGameRoot.Instance.OpenPause = false;
            UIManager.Instance.Close(UIType.PausePanelView);
        }
        
        private void ReturnToWelcomePanel()
        {
            // HGameRoot.Instance.gameStart = false;
            // HGameRoot.Instance.OpenPause = false;
            // Time.timeScale = scaleTime;
            // //todo:这里应该发送事件卸载关卡的所有资源，但这个再说
            // UIManager.Instance.Close(UIType.PausePanelView);
            // //reload scene
            // string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            // UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            // 这个暂时不好做，做成退出游戏好了
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        private void OpenSettingPanel()
        {
            UIManager.Instance.Open(UIType.GameSettingView);
        }

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            ReturnGameBtn.onClick.AddListener(ReturnToGame);
            ReturnWelcomePanel.onClick.AddListener(ReturnToWelcomePanel);
            SettingBtn.onClick.AddListener(OpenSettingPanel);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            scaleTime = Time.timeScale;
            Time.timeScale = 0.01f;
        }

        public override void OnAddListener()
        {
            base.OnAddListener();
        }

        public override void OnRemoveListener()
        {
            base.OnRemoveListener();
        }

        public override void OnClose()
        {
            base.OnClose();
            Time.timeScale = scaleTime;
        }
    }
}
