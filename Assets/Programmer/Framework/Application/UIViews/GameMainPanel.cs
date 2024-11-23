using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OurGameFramework
{
    public class GameMainPanel : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
		#pragma warning disable 0649
		[ControlBinding]
		private Slider SliderCommon1;
		[ControlBinding]
		private Slider SliderCommon2;

		#pragma warning restore 0649
#endregion

        private SliderController sliderController;

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            EventManager.AddEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(), NoticeSliderValueChange);
        }
        
        private void NoticeSliderValueChange(SliderEvent sliderEvent, string sliderName, float addValue)
        {
            //Debug.LogError("now we are here");
            if (sliderController)
            {
                if(sliderEvent == SliderEvent.SLIDER_VALUE_CHANGE)
                    sliderController.ChangeSliderValue(sliderName, addValue, true);
            }
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameMainPanelStruct gameMainPanelStruct = userData as GameMainPanelStruct;
            int levelID = gameMainPanelStruct.levelID;
            //初始化游戏设置
            GameObject gameRoot = HGameRoot.Instance.gameObject;
            sliderController = gameRoot.GetOrAddComponent<SliderController>();
            sliderController.SetSliders(new List<Slider> { SliderCommon1, SliderCommon2 }, levelID);
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
            //清空slider条
            if (sliderController)
            {
                sliderController.ClearSliders();
            }
        }
    }
}
