using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OurGameFramework
{
    public class UILoopItem : UISubView
    {
        protected int m_Index;
        protected RectTransform m_RectTransform;
        public int Index => m_Index;
        public UIScrollView UIScrollView { get; set; }

        public override void OnInit()
        {
            base.OnInit();
            m_RectTransform = transform as RectTransform;
        }

        public void UpdateSingleData(IList dataList, int index, object userData)  //只能单选的情况
        {
            if (!isInit)
            {
                OnInit();
            }
            m_Index = index;
            m_RectTransform.localPosition = Vector3.zero;
            m_RectTransform.anchoredPosition = UIScrollView.GetLocalPositionByIndex(index);
            CheckSelect(UIScrollView.SelectIndex, dataList[index]);
            OnUpdateData(dataList, index, userData);
        }

        public void UpdateData(IList dataList, int index, object userData)  //可以多选的情况
        {
            if (!isInit)
            {
                OnInit();
            }
            m_Index = index;
            m_RectTransform.localPosition = Vector3.zero;
            m_RectTransform.anchoredPosition = UIScrollView.GetLocalPositionByIndex(index);
            //CheckSelect(UIScrollView.SelectIndex);
            GenshinUserDataStruct genshinUserData = userData as GenshinUserDataStruct;
            if(UIScrollView.SelectIndexs.ContainsKey(index))
            {
                CheckSelect(m_Index, dataList[index]);
            }
            else
            {
                CheckSelect(-1, dataList[index]);
            }
            
            OnUpdateData(dataList, index, genshinUserData);
        }

        /// <summary>
        /// 选中切换时
        /// </summary>
        public virtual void CheckSelect(int index)
        {

        }

        public virtual void CheckSelect(int index, object data, bool isRemove = false)
        {
            
        }

        public virtual void SetBaseData()  //比如有一个子节点什么的，可以在这里面进行赋值
        {
            
        }
        

        /// <summary>
        /// Item的宽高
        /// </summary>
        public virtual Vector2 GetRect()
        {
            return new Vector2(m_RectTransform.rect.width * m_RectTransform.localScale.x, m_RectTransform.rect.height * m_RectTransform.localScale.y);
        }

        /// <summary>
        /// 刷新数据时
        /// </summary>
        protected virtual void OnUpdateData(IList dataList, int index, object userData)
        {
            
        }
    }
}
