using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace OurGameFramework
{
    public class LevelWinPanelView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        public TextMeshProUGUI timeUseText;
        [ControlBinding]
        public Button BackToWelcomeBtn;
        [ControlBinding]
        public Button NextLevelBtn;
        [ControlBinding]
        public RectTransform Stars;
        [ControlBinding]
        public TextMeshProUGUI bestShow;
        [ControlBinding]
        public TextMeshProUGUI HistoryBestText;

#pragma warning restore 0649
        #endregion
        
        private int winCurrentID = -1;

        private void EnterNextLevel()
        {
            StartCoroutine(EnterNextLevelCorotine());
        }

        IEnumerator EnterNextLevelCorotine()
        {
            yield return HLevelManager.Instance.EnterNextLevel();
            int levelId = winCurrentID + 1; //todo:写的不太行，先这样，最后一关的时候可能会出问题
            HGameRoot.Instance.currentMaxLevel = levelId;
            
            int totalTimeCount = SD_CatGameLevelConfig.Class_Dic[levelId.ToString()]._levelTotalTime();
            GameMainPanelStruct gameMainPanelStruct = new GameMainPanelStruct();
            gameMainPanelStruct.levelID = levelId;
            gameMainPanelStruct.totalAllowTime = totalTimeCount;
            int bestUseTime = HGameRoot.Instance.playerData.levelBestTimes[levelId - 1];
            gameMainPanelStruct.bestUseTime = bestUseTime;
            UIManager.Instance.Open(UIType.GameMainPanel, gameMainPanelStruct);
        }
        
        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            NextLevelBtn.onClick.AddListener(EnterNextLevel);
            BackToWelcomeBtn.onClick.AddListener(BackToWelcome);
        }

        private void BackToWelcome()
        {
            UIManager.Instance.Open(UIType.GameWelcomePanel);
            HGameRoot.Instance.OpenPause = false;
            UIManager.Instance.Close(UIType.GameMainPanel);
            EventSystem.current.SetSelectedGameObject(null);
            HLevelManager.Instance.ClearAllLevels();
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
            string bestTimeStr;
            if (bestTime != -1)
            {
               bestTimeStr = "历史最快："+FormatIntTimeToString(bestTime);
            }
            else
            {
                bestTimeStr = "历史最快：无";
            }
            
            timeUseText.text = timeUseStr;
            HistoryBestText.text = bestTimeStr;
            if(timeUse<bestTime || bestTime == -1)
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
            winCurrentID = gameOverStruct.levelID;
            int totalLevelCnt = SD_CatGameLevelConfig.Class_Dic.Count;
            if(winCurrentID>=totalLevelCnt)
            {
                NextLevelBtn.interactable = false;
            }
            else
            {
                NextLevelBtn.interactable = true;
            }
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
