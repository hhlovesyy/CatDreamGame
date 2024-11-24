using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OurGameFramework
{
    public class GameSettingView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        public Slider VolumeSlider;
        [ControlBinding]
        public Button ReturnButton;
        [ControlBinding]
        public Slider SensitiveSlider;

#pragma warning restore 0649
        #endregion

        private void ReturnToMain()
        {
            UIManager.Instance.Open(UIType.GameWelcomePanel);
        }

        private void OnVolumeChanged(float value)
        {
            Debug.Log("OnVolumeChanged  " + value);
        }

        private void OnSensitiveChanged(float value)
        {
            Debug.Log("OnSensitiveChanged  " + value);
        }

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            ReturnButton.onClick.AddListener(ReturnToMain);
            VolumeSlider.onValueChanged.AddListener((value) => OnVolumeChanged(value));
            SensitiveSlider.onValueChanged.AddListener((value) => OnSensitiveChanged(value));
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
            // ReturnButton.onClick.RemoveAllListeners();
            // VolumeSlider.onValueChanged.RemoveAllListeners();
            // SensitiveSlider.onValueChanged.RemoveAllListeners();
        }

        public override void OnClose()
        {
            base.OnClose();
        }
    }
}
