using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening.Core.Easing;

namespace OurGameFramework
{
    public class UIGenshinItem : UILoopItem
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        public TextMeshProUGUI Text;
        [ControlBinding]
        public TextMeshProUGUI SelectCount;
        [ControlBinding]
        public TextMeshProUGUI DescriptionText;
        [ControlBinding]
        public Image BgImg;
        [ControlBinding]
        public Button SelectButton;
        [ControlBinding]
        public Button Button;
        [ControlBinding]
        public GameObject Select;

#pragma warning restore 0649
        #endregion
        
        GenshinUserDataStruct localUserData;
        private GenshinDemoListData localData;
        
        public override void OnInit()
        {
            base.OnInit();
            Select.SetActive(false);
            Button.AddClick(() =>
            {
                UIScrollView.NewSelect(Index);
            });
            SelectButton.AddClick(()=>
            {
                UIScrollView.NewSelect(Index, true);
            });
        }

        public override void SetBaseData()
        {
            base.SetBaseData();
            //以下功能用于长按连续选择
            Button.GetComponent<ButtonExtendEvent>().SetScrollViewAndIndex(UIScrollView,this, false); 
            SelectButton.GetComponent<ButtonExtendEvent>().SetScrollViewAndIndex(UIScrollView, this, true);
        }


        public override void CheckSelect(int index, object data, bool isRemove)
        {
            if (isRemove == false)
            {
                GenshinDemoListData genshinData = data as GenshinDemoListData;
                int selectCount = genshinData.selectCount;
                bool isShow = (index == Index);
                Select.SetActive(isShow);
                SelectButton.gameObject.SetActive(isShow);
                SelectCount.gameObject.SetActive(isShow && selectCount > 0 && genshinData.multiSelectInOneItem);
                SelectCount.text = selectCount.ToString();
            }
            else  //是要移除物品
            {
                GenshinDemoListData genshinData = data as GenshinDemoListData;
                int selectCount = genshinData.selectCount;
                bool isShow = selectCount > 0 && (index == Index);
                Select.SetActive(isShow);
                SelectButton.gameObject.SetActive(isShow);
                SelectCount.gameObject.SetActive(isShow && genshinData.multiSelectInOneItem);
                SelectCount.text = selectCount.ToString();
            }
        }
        
        public override void CheckSelect(int index)
        {
            //Debug.Log("CheckSelect:" + index);
            base.CheckSelect(index);
            bool isShow = (index == Index);
            
            Select.SetActive(isShow);
            SelectButton.gameObject.SetActive(isShow && localUserData.isShowX);
            SelectCount.gameObject.SetActive(isShow && localData.selectCount > 0);
        }

        protected override void OnUpdateData(IList dataList, int index, object userData)
        {
            //Debug.Log("OnUpdateData:" + index);
            base.OnUpdateData(dataList, index, userData);
            GenshinDemoListData data = dataList[index] as GenshinDemoListData;
            localData = data;
            Text.text = data.name;
            if(userData != null)
            {
                localUserData = userData as GenshinUserDataStruct;
                ShowDataLogic(data);
            }
        }

        private void ShowDataLogic(GenshinDemoListData data)
        {
            DescriptionText.text = data.count.ToString();
            switch (data.type)
            {
                case GenshinDemoListType.Weapon:
                    DescriptionText.text = "Lv." + data.level;
                    break;
                case GenshinDemoListType.Mineral: //矿物
                    DescriptionText.text = data.count.ToString();
                    break;
            }
            SelectCount.text = data.selectCount.ToString();
        }
    }
    public class UITestItem : UILoopItem
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        private TextMeshProUGUI Text;
        [ControlBinding]
        private Button Button;
        [ControlBinding]
        private GameObject Select;

#pragma warning restore 0649
        #endregion

        public override void OnInit()
        {
            base.OnInit();
            Button.AddClick(() =>
            {
                UIScrollView.NewSelect(Index);
            });
        }

        public override void CheckSelect(int index)
        {
            base.CheckSelect(index);
            Select.SetActive(index == Index);
        }

        protected override void OnUpdateData(IList dataList, int index, object userData)
        {
            base.OnUpdateData(dataList, index, userData);

            Text.text = dataList[index].ToString();
        }
    }

    public class UILoginView : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        private Button ButtonStart;
        [ControlBinding]
        private Button ButtonSetting;
        [ControlBinding]
        private UIScrollView UIScrollView;
        [ControlBinding]
        private GameObject Item;
        [ControlBinding]
        private RawImage RawImage;

#pragma warning restore 0649
        #endregion

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);

            ButtonStart.AddClick(() =>
            {
                UIManager.Instance.Open(UIType.UIMessageBoxView, ObjectPool<MessageBoxData>.Get().Set("提示", "测试弹窗。", () =>
                {
                    Debug.Log("确认");
                }));
            });
            ButtonSetting.AddClick(() =>
            {
                UIManager.Instance.Open(UIType.UITestView);
            });

            UIScrollView.OnSelectChanged += (index) =>
            {
                Debug.Log("选中了：" + index);
            };
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            // 模拟100个数据
            List<int> list = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(i);
            }
            UIScrollView.SetUpList(list, Item, typeof(UITestItem));
            UIScrollView.Select(10);

            UIModelManager.Instance.LoadModelToRawImage("Assets/AssetsPackage/TestModel.prefab", RawImage, scale: Vector3.one * 6, isOrth: false, orthSizeOrFOV: 60);
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
            UIModelManager.Instance.UnLoadModelByRawImage(RawImage);
        }
    }
}
