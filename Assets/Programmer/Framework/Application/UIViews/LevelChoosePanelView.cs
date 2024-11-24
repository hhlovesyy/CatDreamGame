using System;
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
        public Button EnterLevelBtn;

#pragma warning restore 0649
        #endregion


        private CatGamePlayerData playerData;
        private int levelId = -1;
        private FancyScrollView.Example09.ScrollView thisScrollView;

        private void EnterLevel()
        {
            Debug.Log("Enter Level! level is: " + levelId);
            if (levelId == -1)
            {
                Debug.LogError("请选择一个关卡");
                return;
            }
            GameMainPanelStruct gameMainPanelStruct = new GameMainPanelStruct();
            gameMainPanelStruct.levelID = levelId;
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

        private void OnPositionChange(int index)
        {
            levelId = index + 1;
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
            thisScrollView.onPositionChange -= OnPositionChange;
        }
    }
}
