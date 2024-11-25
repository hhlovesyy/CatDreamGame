using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OurGameFramework
{
    public class LevelWinPanelView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
		#pragma warning disable 0649
		[ControlBinding]
		private TextMeshProUGUI timeUseText;
		[ControlBinding]
		private RectTransform Stars;
		[ControlBinding]
		private TextMeshProUGUI bestShow;
		[ControlBinding]
		private TextMeshProUGUI HistoryBestText;

		#pragma warning restore 0649
#endregion

        

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
        }
        
        private string FormatIntTimeToString(int time)
        {
            //将时间转换成字符串，格式为00:00，即分：秒，两位
            string timeStr = "";
            int minute = time / 60;
            int second = time % 60;
            timeStr = minute.ToString("00") + ":" + second.ToString("00");
            return timeStr;
        }

        private void ShowResult(GameOverStruct gameOverStruct)
        {
            int timeUse = gameOverStruct.totalUseTime;
            int bestTime = gameOverStruct.bestUseTime;
            int starLevel = gameOverStruct.starLevel;
            string timeUseStr = "本关用时："+FormatIntTimeToString(timeUse);
            string bestTimeStr = "历史最快："+FormatIntTimeToString(bestTime);
            timeUseText.text = timeUseStr;
            HistoryBestText.text = bestTimeStr;
            if(timeUse<bestTime)
            {
                bestShow.gameObject.SetActive(true);
                bestShow.transform.DOShakeScale(1f);
            }
            else
            {
                bestShow.gameObject.SetActive(false);
            }
            //星级显示，先全部默认白色
            for(int i=0;i<Stars.childCount;i++)
            {
                Stars.gameObject.transform.GetChild(i).GetComponent<RawImage>().color = Color.white;
            }
            //根据星级显示星星
            for(int i=0;i<starLevel;i++)
            {
                Stars.gameObject.transform.GetChild(i).GetComponent<RawImage>().color = Color.yellow;
                Stars.gameObject.transform.GetChild(i).DOShakeRotation(1f);
            }
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameOverStruct gameOverStruct = userData as GameOverStruct;
            ShowResult(gameOverStruct);
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
