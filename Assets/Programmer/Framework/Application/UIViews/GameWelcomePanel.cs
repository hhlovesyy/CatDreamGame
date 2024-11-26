using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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
            HGameRoot.Instance.gameStart = true;
            //UIManager.Instance.Open(UIType.GameMainPanel, gameMainPanelStruct);
            UIManager.Instance.Open(UIType.LevelChoosePanelView, HGameRoot.Instance.playerData);
        }

        private void OpenSettings()
        {
            Debug.Log("Open Settings!");
            EventSystem.current.SetSelectedGameObject(null);  //note：这句话比较关键，解决了打开设置界面并返回后，设置按钮默认被按下的问题
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
            HAudioManager.Instance.Play("WelcomePanelMusic",HGameRoot.Instance.gameObject);
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
