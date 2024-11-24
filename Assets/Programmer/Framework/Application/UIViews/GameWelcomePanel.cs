using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OurGameFramework
{
    public class GameWelcomePanel : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
		#pragma warning disable 0649
		[ControlBinding]
		private Button StartGameBotton;
		[ControlBinding]
		private Button SettingButton;
		[ControlBinding]
		private Button ExitButton;

		#pragma warning restore 0649
        #endregion


        private void StartGame()
        {
            Debug.Log("Start Game!");
            GameMainPanelStruct gameMainPanelStruct = new GameMainPanelStruct();
            gameMainPanelStruct.levelID = 1;
            UIManager.Instance.Open(UIType.GameMainPanel, gameMainPanelStruct);
        }

        private void OpenSettings()
        {
            Debug.Log("Open Settings!");
            UIManager.Instance.Open(UIType.GameSettingView);
        }

        private void ExitGame()
        {
            Debug.Log("Exit Game!");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            StartGameBotton.onClick.AddListener(StartGame);
            SettingButton.onClick.AddListener(OpenSettings);
            ExitButton.onClick.AddListener(ExitGame);
            
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        public override void OnAddListener()
        {
            base.OnAddListener();
        }

        public override void OnRemoveListener()
        {
            base.OnRemoveListener();
            // StartGameBotton.onClick.RemoveListener(StartGame);
            // SettingButton.onClick.RemoveListener(OpenSettings);
            // ExitButton.onClick.RemoveListener(ExitGame);
        }

        public override void OnClose()
        {
            base.OnClose();
        }
    }
}
