using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace OurGameFramework
{
    public class GameSettingView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        public Slider VolumeSlider;
        [ControlBinding]
        public Button ReturnButton2;
        [ControlBinding]
        public TextMeshProUGUI VolumeShow;
        [ControlBinding]
        public TextMeshProUGUI SensitiveShow;
        [ControlBinding]
        public Button ReturnButton;
        [ControlBinding]
        public Slider SensitiveSlider;

#pragma warning restore 0649
        #endregion
        
        HGameRoot gameRoot;

        private void CloseSettingPanel()
        {
            UIManager.Instance.Close(UIType.GameSettingView);
        }

        private void OnVolumeChanged(float value)
        {
            //Debug.Log("OnVolumeChanged  " + value);
            //todo:后面做音频部分的时候再改，这里应该用事件系统来通知AudioManager来做音量的调整
            if (gameRoot)
            {
                gameRoot.VolumeMultiplier = value;  
                HAudioManager.Instance.VolumeMultiplier = value;
                HAudioManager.Instance.UpdateAllAudioVolumes();
                float showValue = (float)Math.Round(gameRoot.VolumeMultiplier, 1);
                float uiValue = showValue * 10;
                VolumeShow.text = uiValue.ToString();
            }
        }

        private void OnSensitiveChanged(float value)
        {
            //Debug.Log("OnSensitiveChanged  " + value);
            if (gameRoot)
            {
                gameRoot.MouseSensitive = value;
                float showValue = (float)Math.Round(gameRoot.MouseSensitive, 1);
                SensitiveShow.text = showValue.ToString();
            }
        }

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            ReturnButton.onClick.AddListener(CloseSettingPanel);
            VolumeSlider.onValueChanged.AddListener((value) => OnVolumeChanged(value));
            SensitiveSlider.onValueChanged.AddListener((value) => OnSensitiveChanged(value));
            ReturnButton2.onClick.AddListener(CloseSettingPanel);
            gameRoot = HGameRoot.Instance.gameObject.GetComponent<HGameRoot>();
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (gameRoot)
            {
                VolumeSlider.value = gameRoot.VolumeMultiplier;
                SensitiveSlider.value = gameRoot.MouseSensitive;
                
                //显示小数点后一位即可
                float showValue = (float)Math.Round(gameRoot.VolumeMultiplier, 1);
                float uiVolume = showValue * 10;
                VolumeShow.text = uiVolume.ToString();
                showValue = (float)Math.Round(gameRoot.MouseSensitive, 1);
                SensitiveShow.text = showValue.ToString();
                
            }
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
