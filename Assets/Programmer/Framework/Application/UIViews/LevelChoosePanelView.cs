using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using FancyScrollView.Example09;

namespace OurGameFramework
{
    public class LevelChoosePanelView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        public GameObject ScrollView;
        [ControlBinding]
        public RawImage LockIcon;
        [ControlBinding]
        public RectTransform Stars;
        [ControlBinding]
        public TextMeshProUGUI FastTimeText;
        [ControlBinding]
        public Button EnterLevelBtn;

#pragma warning restore 0649
        #endregion
        
        private CatGamePlayerData playerData;
        private int levelId = -1;
        private int lastLevelId = -1;
        private FancyScrollView.Example09.ScrollView thisScrollView;

        private void EnterLevel()
        {
            Debug.Log("Enter Level! level is: " + levelId);
            if (levelId == -1)
            {
                Debug.LogError("请选择一个关卡");
                return;
            }

            StartCoroutine(EnterThisLevel());
        }

        IEnumerator EnterThisLevel()
        {
            yield return HLevelManager.Instance.LoadOneLevel(levelId);
            int totalTimeCount = SD_CatGameLevelConfig.Class_Dic[levelId.ToString()]._levelTotalTime();
            GameMainPanelStruct gameMainPanelStruct = new GameMainPanelStruct();
            gameMainPanelStruct.levelID = levelId;
            gameMainPanelStruct.totalAllowTime = totalTimeCount;
            int bestUseTime = playerData.levelBestTimes[levelId - 1];
            gameMainPanelStruct.bestUseTime = bestUseTime;
            UIManager.Instance.Open(UIType.GameMainPanel, gameMainPanelStruct);
        }
        
        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            EnterLevelBtn.onClick.AddListener(EnterLevel);
            thisScrollView = ScrollView.GetComponent<FancyScrollView.Example09.ScrollView>();
            thisScrollView.onPositionChange += OnPositionChange;
        }

        private void InitializeLevelInfo()
        {
            //从策划表中读取每个关卡的基本信息
            Dictionary<string, Class_CatGameLevelConfig> levelConfigDic = SD_CatGameLevelConfig.Class_Dic;
            int totalLevel = levelConfigDic.Count;
            List<ItemData> levelItemDataList = new List<ItemData>();
            for (int i = 1; i <= totalLevel; i++)
            {
                string title = levelConfigDic[i.ToString()].levelTitle;
                string description = levelConfigDic[i.ToString()].levelDescription;
                string url = levelConfigDic[i.ToString()].levelIconAddressable;
                ItemData levelItemData = new ItemData(title, description, url);
                levelItemDataList.Add(levelItemData);
            }
            //将关卡信息传递给ScrollView
            thisScrollView.UpdateData(levelItemDataList);
        }

        private string ConvertIntToTimeString(int time)
        {
            int minute = time / 60;
            int second = time % 60;
            return "最快用时：" + minute.ToString() + ":" + second.ToString();
        }

        private void UpdateInfo()
        {
            if (lastLevelId == levelId) return;
            if (playerData != null)
            {
                lastLevelId = levelId;
                int findId = levelId - 1;
                if(findId >= playerData.levelBestTimes.Count)
                    findId = playerData.levelBestTimes.Count - 1;
                if(findId < 0)
                    findId = 0;
                int currentMaxLevel = HGameRoot.Instance.currentMaxLevel;
                int currentLevelIndex = findId + 1;
                bool isLock = currentLevelIndex > currentMaxLevel;
                LockIcon.gameObject.SetActive(isLock);
                EnterLevelBtn.interactable = !isLock;
                FastTimeText.gameObject.SetActive(!isLock);
                Stars.gameObject.SetActive(!isLock);
                
                //如果是-1则不显示，说明还没有存档
                if (playerData.levelBestTimes[findId] == -1)
                {
                    FastTimeText.gameObject.SetActive(false);
                    Stars.gameObject.SetActive(false);
                }
                
                FastTimeText.text = ConvertIntToTimeString(playerData.levelBestTimes[findId]);
                //全部重置为白色
                for(int i=0;i<Stars.childCount;i++)
                {
                    Stars.GetChild(i).gameObject.GetComponent<RawImage>().color = Color.white;
                }
                for(int i=0;i<playerData.levelBestStars[findId];i++)
                {
                    Stars.GetChild(i).gameObject.GetComponent<RawImage>().color = Color.yellow;
                }
            }
        }

        private void OnPositionChange(int index)
        {
            levelId = index + 1;
            UpdateInfo();
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            playerData = userData as CatGamePlayerData;
            InitializeLevelInfo();
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
            //thisScrollView.onPositionChange -= OnPositionChange;
        }
    }
}
