using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OurGameFramework
{
    public class TutorialPagePanel : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
		#pragma warning disable 0649
		[ControlBinding]
		private Button CloseBtn;

		#pragma warning restore 0649
#endregion

        private void CloseTutorial()
        {
            HAudioManager.Instance.Play("ButtonClickAudio", Camera.main.gameObject);
            UIManager.Instance.Close(UIType.TutorialPagePanel);
        }

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            CloseBtn.onClick.AddListener(CloseTutorial);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //Time.timeScale = 0.01f;
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
        }
    }
}
