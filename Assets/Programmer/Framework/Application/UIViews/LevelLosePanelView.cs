using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace OurGameFramework
{
    public class LevelLosePanelView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
		#pragma warning disable 0649
		[ControlBinding]
		private TextMeshProUGUI LoseTip;
		[ControlBinding]
		private Button RestartLevelBtn;
		[ControlBinding]
		private Button BackToWelcomeBtn;

		#pragma warning restore 0649
#endregion
        
        private int currentLevelID = -1;
        private void BackToWelcome()
        {
            HAudioManager.Instance.Play("ButtonClickAudio", Camera.main.gameObject);
            UIManager.Instance.Open(UIType.GameWelcomePanel);
            HGameRoot.Instance.OpenPause = false;
            UIManager.Instance.Close(UIType.GameMainPanel);
            EventSystem.current.SetSelectedGameObject(null);
            HLevelManager.Instance.ClearAllLevels();
        }

        private void RestartThisLevel()
        {
            StopAllCoroutines();
            HAudioManager.Instance.Play("ButtonClickAudio", Camera.main.gameObject);
            StartCoroutine(EnterThisLevelCoroutine());
        }
        
        IEnumerator EnterThisLevelCoroutine()
        {
            yield return HLevelManager.Instance.RestartEnterThisLevel();
            int levelId = currentLevelID;
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
            RestartLevelBtn.onClick.AddListener(RestartThisLevel);
            BackToWelcomeBtn.onClick.AddListener(BackToWelcome);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameOverStruct gameoverStruct = (GameOverStruct)userData;
            int loseLevelId = gameoverStruct.levelID;
            currentLevelID = loseLevelId;
            // 从策划表中拿出鼓励话语
            string loseTip = SD_CatGameLevelConfig.Class_Dic[loseLevelId.ToString()]._LevelLoseTip();
            //用协程讲这句话一个一个字打出来
            StartCoroutine(ShowLoseTip(loseTip));
            HAudioManager.Instance.Play("LevelLooseAudio", this.gameObject);
        }
        
        private IEnumerator ShowLoseTip(string loseTip)
        {
            LoseTip.text = "";
            for (int i = 0; i < loseTip.Length; i++)
            {
                LoseTip.text += loseTip[i];
                yield return new WaitForSeconds(0.1f);
            }
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
