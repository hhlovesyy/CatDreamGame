using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.RectTransform;
using DG.Tweening;
using Unity.VisualScripting;

namespace OurGameFramework
{
    public enum AlignType
    {
        Left,
        Right,
        Top,
        Bottom,
        Center,
    }

    public class UIScrollView : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
    {
        public ScrollRect m_ScrollRect;
        public RectTransform m_Content;

        /// <summary>
        /// 水平移动/垂直移动
        /// </summary>
        public Axis m_AxisType;

        /// <summary>
        /// 布局
        /// </summary>
        public AlignType m_AlignType;

        /// <summary>
        /// 子物体的中心点
        /// </summary>
        public PivotPresets m_ItemPivot;

        /// <summary>
        /// 开始间隔
        /// </summary>
        public int m_HorizontalStartSpace;

        /// <summary>
        /// 开始间隔
        /// </summary>
        public int m_VerticalStartSpace;

        /// <summary>
        /// 水平间隔
        /// </summary>
        public int m_HorizontalSpace;

        /// <summary>
        /// 垂直间隔
        /// </summary>
        public int m_VerticalSpace;

        /// <summary>
        /// 另一个方向上物品的个数，水平移动-表示列个数，垂直移动-表示行个数
        /// </summary>
        public int m_CountOfOtherAxis = 1;

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool m_IsPaging;

        private int m_maxChooseCnt = 10000; // 默认不做限制

        private IList m_Datas;
        private PrefabPool m_PrefabPool;
        private List<UILoopItem> m_LoopItems;
        private int m_SelectIndex = -1;
        private Dictionary<int, int> m_SelectIndexs = new Dictionary<int, int>();

        private int m_HorizontalCount;
        private int m_VerticalCount;
        private float m_ChildWidth;
        private float m_ChildHeight;
        private int m_CurrentIndex;

        private Type m_ItemType;
        private object m_UserData;

        private Tweener m_Tweener;

        public Action<int> OnSelectChanged;
        public int SelectIndex => m_SelectIndex;
        public List<UILoopItem> LoopItems => m_LoopItems;
        public int CurrentIndex => m_CurrentIndex;
        public Dictionary<int, int> SelectIndexs => m_SelectIndexs;

        private Rect parentRect;

        private Coroutine m_LongPressCoroutine;
        private Coroutine m_HoldRangeCoroutine;
        private bool m_canRangeSelect = false;

        private void Awake()
        {
            m_LoopItems = new List<UILoopItem>();
            //以下回调函数会在滑动滚动条，拖动滚动条的时候调用，以及由于重新实现了点击重新布局的功能，因此点击内部某个按钮的时候也会回调这个函数
            m_ScrollRect.onValueChanged.AddListener(OnValueChanged);
            if (m_AxisType != Axis.Horizontal)
                m_ScrollRect.horizontal = false;
            if (m_AxisType != Axis.Vertical)
                m_ScrollRect.vertical = false;
        }

        public void AddSelectIndex(int index, int maxCount, bool multiSelectInOneItem = false)
        {
            //dictionary的key是index，value是点击的次数
            if (!m_SelectIndexs.ContainsKey(index))
            {
                m_SelectIndexs.Add(index, 1);
            }
            else
            {
                if (!multiSelectInOneItem) //本身不能多选
                {
                    m_SelectIndexs[index] = 1;
                    return;
                }
                if (m_SelectIndexs[index] < maxCount)
                {
                    m_SelectIndexs[index]++;
                }
            }
        }

        void RefleshSingleData(int index)
        {
            GenshinDemoListData data = m_Datas[index] as GenshinDemoListData;
            data.selectCount = m_SelectIndexs[index];
        }

        public void RemoveSelectIndex(int index, bool multiSelectInOneItem = false) 
        {
            //dictionary的key是index，value是点击的次数,如果点击次数为0，就删除这个index
            if (m_SelectIndexs.ContainsKey(index))
            {
                m_SelectIndexs[index]--;
                if(!multiSelectInOneItem) m_SelectIndexs[index] = 0; //单选情况，删除即为0
                RefleshSingleData(index);
                if (m_SelectIndexs[index] <= 0)
                {
                    m_SelectIndexs.Remove(index);
                }
            }
        }

        /// <summary>
        /// 刷新整个scrollview
        /// </summary>
        /// <param name="dataList">数据</param>
        /// <param name="prefab">预制体</param>
        /// <param name="type">类型</param>
        /// <param name="isPaging">是否分页</param>
        /// <param name="userData">用户数据</param>
        public void SetUpList(IList dataList, GameObject prefab, Type type, bool isKeepPos = false,
            object userData = null)
        {
            if (dataList == null || prefab == null)
            {
                Release();
                return;
            }

            if (m_PrefabPool != null && m_PrefabPool.Prefab != prefab)
            {
                m_PrefabPool.Destroy();
                m_PrefabPool = null;
            }

            if (m_PrefabPool == null)
            {
                m_PrefabPool = PrefabPool.Create(prefab);
            }

            prefab.SetActive(false);
            m_PrefabPool.RecycleUseList();
            m_LoopItems.Clear();
            m_SelectIndex = -1;

            RectTransform rect = prefab.GetComponent<RectTransform>();
            m_ChildWidth = rect.rect.width * rect.transform.localScale.x;
            m_ChildHeight = rect.rect.height * rect.transform.localScale.y;

            var parent = m_ScrollRect.transform as RectTransform;
            parentRect = parent.rect;
            m_HorizontalCount =
                Mathf.CeilToInt((parentRect.width - m_HorizontalStartSpace) / (m_ChildWidth + m_HorizontalSpace));
            m_VerticalCount =
                Mathf.CeilToInt((parentRect.height - m_VerticalStartSpace) / (m_ChildHeight + m_VerticalSpace));

            m_ItemType = type;
            m_Datas = dataList;
            m_UserData = userData;
            
            GenshinUserDataStruct data = userData as GenshinUserDataStruct;
            //尝试userData转为GenshinUserDataStruct
            if (userData != null)
            {
                m_maxChooseCnt = data.maxSelectCount;
                m_canRangeSelect = data.canRangeSelect;
            }

            m_Content.SetPivot(m_ItemPivot);
            Vector2 oldPos = m_Content.anchoredPosition;

            if (m_Tweener != null)
            {
                m_Tweener.Kill();
                m_Tweener = null;
            }

            if (m_CountOfOtherAxis == 0)
            {
                //todo：如果m_CountOfOtherAxis比较大，每个Item的大小不改的话会显示不全，这个问题要修复一下
                if (m_AxisType == Axis.Horizontal)
                    m_CountOfOtherAxis = Mathf.FloorToInt((parentRect.height - m_VerticalStartSpace) /
                                                          (m_ChildHeight + m_VerticalSpace));
                else
                    m_CountOfOtherAxis = Mathf.FloorToInt((parentRect.width - m_HorizontalStartSpace) /
                                                          (m_ChildWidth + m_HorizontalSpace));

                m_CountOfOtherAxis = Math.Max(1, m_CountOfOtherAxis);
            }

            if (m_AxisType == Axis.Horizontal)
                m_VerticalCount = m_CountOfOtherAxis;
            else
                m_HorizontalCount = m_CountOfOtherAxis;

            int axisCount = Mathf.CeilToInt(dataList.Count * 1.0f / m_CountOfOtherAxis);
            switch (m_AxisType)
            {
                case Axis.Horizontal:
                    if (m_AlignType == AlignType.Right)
                    {
                        m_Content.SetAnchor(AnchorPresets.VertStretchRight);
                    }
                    else
                    {
                        m_Content.SetAnchor(AnchorPresets.VertStretchLeft);
                    }

                    m_Content.sizeDelta =
                        new Vector2(
                            axisCount * m_ChildWidth + (axisCount - 1) * m_HorizontalSpace + m_HorizontalStartSpace * 2,
                            0);
                    if (m_AlignType == AlignType.Center)
                    {
                        var viewPort = m_Content.parent as RectTransform;
                        viewPort.anchorMin = new Vector2(0.5f, 0.5f);
                        viewPort.anchorMax = new Vector2(0.5f, 0.5f);
                        viewPort.pivot = new Vector2(0.5f, 0.5f);
                        viewPort.anchoredPosition = Vector2.zero;
                        viewPort.sizeDelta = new Vector2(m_Content.sizeDelta.x, parentRect.height);
                        int verCount = Mathf.FloorToInt((parentRect.height - m_VerticalStartSpace) /
                                                        (m_ChildHeight + m_VerticalSpace));
                        if (verCount > m_Datas.Count)
                        {
                            viewPort.sizeDelta = new Vector2(m_Content.sizeDelta.x,
                                (m_ChildHeight + m_VerticalSpace) * m_Datas.Count - m_VerticalSpace +
                                m_VerticalStartSpace * 2);
                        }
                    }

                    break;
                case Axis.Vertical:
                    if (m_AlignType == AlignType.Bottom)
                    {
                        m_Content.SetAnchor(AnchorPresets.BottomStretch);
                    }
                    else
                    {
                        m_Content.SetAnchor(AnchorPresets.HorStretchTop);
                    }

                    m_Content.sizeDelta = new Vector2(0,
                        axisCount * m_ChildHeight + (axisCount - 1) * m_VerticalSpace + m_VerticalStartSpace * 2);
                    if (m_AlignType == AlignType.Center)
                    {
                        var viewPort = m_Content.parent as RectTransform;
                        viewPort.anchorMin = new Vector2(0.5f, 0.5f);
                        viewPort.anchorMax = new Vector2(0.5f, 0.5f);
                        viewPort.pivot = new Vector2(0.5f, 0.5f);
                        viewPort.anchoredPosition = Vector2.zero;
                        viewPort.sizeDelta = new Vector2(parentRect.width, m_Content.sizeDelta.y);
                        int horCount = Mathf.CeilToInt((parentRect.width - m_HorizontalStartSpace) /
                                                       (m_ChildWidth + m_HorizontalSpace));
                        if (horCount > m_Datas.Count)
                        {
                            viewPort.sizeDelta =
                                new Vector2(
                                    (m_ChildWidth + m_HorizontalSpace) * m_Datas.Count - m_HorizontalSpace +
                                    m_HorizontalStartSpace * 2, m_Content.sizeDelta.y);
                        }
                    }

                    break;
            }

            if (isKeepPos)
            {
                m_Content.anchoredPosition = new Vector2(Mathf.Min(oldPos.x, m_Content.sizeDelta.x),
                    Mathf.Min(oldPos.y, m_Content.sizeDelta.y));
            }
            else
            {
                m_Content.anchoredPosition = Vector2.zero;
            }

            m_CurrentIndex = GetCurrentItemIndex();
            UpdateContent(m_CurrentIndex);
        }

        public void GetPos(int index, out int x, out int y)
        {
            if (m_AxisType == Axis.Horizontal)
            {
                x = index / m_CountOfOtherAxis;
                y = index % m_CountOfOtherAxis;
            }
            else
            {
                x = index % m_CountOfOtherAxis;
                y = index / m_CountOfOtherAxis;
            }
        }

        public int GetIndex(int x, int y)
        {
            if (x < 0 || y < 0) return -1;

            if (m_AxisType == Axis.Horizontal)
            {
                if (y >= m_CountOfOtherAxis) return -1;

                return x * m_CountOfOtherAxis + y;
            }
            else
            {
                if (x >= m_CountOfOtherAxis) return -1;

                return y * m_CountOfOtherAxis + x;
            }
        }

        public void UpdateContent(int index = 0)
        {
            if (m_Datas == null) return;
            int maxCount = 0;
            switch (m_AxisType)
            {
                case Axis.Horizontal:
                    maxCount = (m_HorizontalCount + 2) * m_CountOfOtherAxis;
                    break;
                case Axis.Vertical:
                    maxCount = (m_VerticalCount + 2) * m_CountOfOtherAxis;
                    break;
            }

            for (int i = 0; i < maxCount; i++)
            {
                int listIndex = index + i;
                //int listIndex = i;
                if (listIndex < m_Datas.Count) //还没有到数据的最后一个
                {
                    if (i < m_LoopItems.Count)
                    {
                        if (m_maxChooseCnt == 1)
                        {
                            m_LoopItems[i].UpdateSingleData(m_Datas, listIndex, m_UserData);
                        }
                        else
                        {
                            m_LoopItems[i].UpdateData(m_Datas, listIndex, m_UserData); //listIndex是比如2000这种值
                        }
                    }
                    else
                    {
                        //超出了已有的格子数量，需要新建对应的格子，只有第一次创建背包格子的时候涉及这个问题
                        var go = m_PrefabPool.Get();
                        RectTransform rectTransform = go.transform as RectTransform;
                        rectTransform.SetPivot(m_ItemPivot);
                        switch (m_ItemPivot)
                        {
                            case PivotPresets.TopLeft:
                            case PivotPresets.TopCenter:
                                rectTransform.SetAnchor(AnchorPresets.TopLeft);
                                break;
                            case PivotPresets.TopRight:
                                rectTransform.SetAnchor(AnchorPresets.TopRight);
                                break;
                            case PivotPresets.MiddleLeft:
                            case PivotPresets.MiddleCenter:
                                rectTransform.SetAnchor(AnchorPresets.MiddleLeft);
                                break;
                            case PivotPresets.MiddleRight:
                                rectTransform.SetAnchor(AnchorPresets.MiddleRight);
                                break;
                            case PivotPresets.BottomLeft:
                            case PivotPresets.BottomCenter:
                                rectTransform.SetAnchor(AnchorPresets.BottomLeft);
                                break;
                            case PivotPresets.BottomRight:
                                rectTransform.SetAnchor(AnchorPresets.BottomRight);
                                break;
                            default:
                                break;
                        }

                        rectTransform.SetParent(m_Content);
                        rectTransform.localScale = m_PrefabPool.Prefab.transform.localScale;
                        UILoopItem loopItem = go.GetOrAddComponent(m_ItemType) as UILoopItem;
                        loopItem.UIScrollView = this;
                        loopItem.SetBaseData();
                        m_LoopItems.Add(loopItem); //todo：有优化空间
                        if (m_maxChooseCnt == 1)
                        {
                            loopItem.UpdateSingleData(m_Datas, listIndex, m_UserData);
                        }
                        else
                        {
                            loopItem.UpdateData(m_Datas, listIndex, m_UserData);
                        }
                    }
                }
                else if (i < m_LoopItems.Count)
                {
                    m_LoopItems[i].transform.localPosition = new Vector3(-10000, -10000);
                }
            }

            while (m_LoopItems.Count > maxCount)
            {
                UILoopItem loopItem = m_LoopItems[m_LoopItems.Count - 1];
                m_PrefabPool.Recycle(loopItem.gameObject);
                m_LoopItems.RemoveAt(m_LoopItems.Count - 1);
            }
        }

        public Vector3 GetLocalPositionByIndex(int index)
        {
            float x, y, z;
            x = y = z = 0.0f;

            int remain = index % m_CountOfOtherAxis;
            index /= m_CountOfOtherAxis;
            switch (m_AxisType)
            {
                case Axis.Horizontal:
                    y = -m_VerticalStartSpace - remain * (m_ChildHeight + m_VerticalSpace);
                    switch (m_AlignType)
                    {
                        case AlignType.Center:
                        case AlignType.Left:
                        case AlignType.Top:
                            x = m_HorizontalStartSpace + index * (m_ChildWidth + m_HorizontalSpace);
                            break;
                        case AlignType.Right:
                        case AlignType.Bottom:
                            x = m_HorizontalStartSpace - index * (m_ChildWidth + m_HorizontalSpace);
                            break;
                        default:
                            break;
                    }

                    break;
                case Axis.Vertical:
                    x = m_HorizontalStartSpace + remain * (m_ChildWidth + m_HorizontalSpace);
                    switch (m_AlignType)
                    {
                        case AlignType.Center:
                        case AlignType.Left:
                        case AlignType.Top:
                            y = -m_VerticalStartSpace - index * (m_ChildHeight + m_VerticalSpace);
                            break;
                        case AlignType.Right:
                        case AlignType.Bottom:
                            y = m_VerticalStartSpace + index * (m_ChildHeight + m_VerticalSpace);
                            break;
                        default:
                            break;
                    }

                    break;
            }

            return new Vector3(x, y, z);
        }

        private void OnValueChanged(Vector2 vec)
        {
            //Debug.Log(vec);  //vec是滚动条的位置，x是0-1，y是0-1
            int index = GetCurrentItemIndex();
            //Debug.Log("now index:" + index);
            if (m_CurrentIndex != index)
            {
                m_CurrentIndex = index;
                UpdateContent(index);
            }
        }

        private int GetCurrentItemIndex()
        {
            // 这个index指的是界面的index，比如说我是8个一行，那么index就是比如8，16，24这样的，可以看log的值
            int index = 0;
            switch (m_AxisType)
            {
                case Axis.Horizontal:
                    if (m_AlignType == AlignType.Left && m_Content.anchoredPosition.x >= 0) return 0;
                    if (m_AlignType == AlignType.Right && m_Content.anchoredPosition.x <= 0) return 0;
                    index = Mathf.FloorToInt((Mathf.Abs(m_Content.anchoredPosition.x) - m_HorizontalStartSpace) /
                                             (m_ChildWidth + m_HorizontalSpace)) * m_CountOfOtherAxis;
                    break;
                case Axis.Vertical:
                    if (m_AlignType == AlignType.Bottom && m_Content.anchoredPosition.y >= 0) return 0;
                    if (m_AlignType == AlignType.Top && m_Content.anchoredPosition.y <= 0) return 0;
                    index = Mathf.FloorToInt((Mathf.Abs(m_Content.anchoredPosition.y) - m_VerticalStartSpace) /
                                             (m_ChildHeight + m_VerticalSpace)) * m_CountOfOtherAxis;
                    break;
            }

            //Debug.Log("current index:" + index);
            return Mathf.Max(0, index);
        }

        public void Select(int index)
        {
            if (m_Datas == null) return;

            if (m_SelectIndex == index) return;

            m_SelectIndex = index;

            foreach (var item in m_LoopItems)
            {
                item.CheckSelect(index);
            }

            if (index >= 0)
            {
                int maxCount = m_AxisType == Axis.Horizontal ? m_HorizontalCount : m_VerticalCount;
                int other = m_AxisType == Axis.Horizontal ? m_VerticalCount : m_HorizontalCount;
                MoveTo(index - (maxCount - 1) * other / 2);
            }

            OnSelectChanged?.Invoke(index);
        }

        private bool m_IsHoldingPressed = false;
        private int startSelectIndex = -1;
        private int endSelectIndex = -1;
        private List<int> tmpChooseIndexs = new List<int>(); //用于存储范围选择的index

        public void StartLongPressSelect(int index, bool isDiSelect = false)
        {
            GenshinDemoListData data = m_Datas[index] as GenshinDemoListData;
            if (firstJudgeDirection)  //drag的时候只会进一次，判断初始点击的那个button是什么
            {
                startSelectIndex = index;
                firstBeingSelected = m_SelectIndexs.ContainsKey(startSelectIndex);
                Debug.LogError("firstBeingSelected:" + firstBeingSelected);
            }
            if (!data.multiSelectInOneItem) return;
            if (m_LongPressCoroutine != null)
            { 
                StopCoroutine(m_LongPressCoroutine);
            }

            if (m_HoldRangeCoroutine != null)
            {
                StopCoroutine(m_HoldRangeCoroutine);
            }

            m_IsHoldingPressed = true;
            m_LongPressCoroutine = StartCoroutine(LongPressSelect(index, isDiSelect));
        }

        public void EndLongPressSelect()
        {
            m_IsHoldingPressed = false;

            if (m_LongPressCoroutine != null)
            {
                StopCoroutine(m_LongPressCoroutine);
            }
        }

        IEnumerator LongPressSelect(int index, bool isDiSelect)
        {
            //Debug.LogError("Now we are in LongPressSelect");
            if (!m_IsHoldingPressed) yield break;
            int checkIndex = index - GetCurrentItemIndex();
            if (checkIndex >= 0 && checkIndex < m_LoopItems.Count)
            {
                m_LoopItems[checkIndex].CheckSelect(index, m_Datas[index]);
            }

            yield return new WaitForSeconds(0.5f); //等待两秒
            if (!m_IsHoldingPressed) yield break;
            GenshinDemoListData data = m_Datas[index] as GenshinDemoListData;
            int upperCnt = data.count;
            int selectCnt = data.selectCount;
            int selectCntStage = 1;
            int thisSelectCnt = 0; //这次选择的数量,每次都是从慢到快
            int delta = isDiSelect ? -1 : 1;
            while (true)
            {
                selectCntStage = thisSelectCnt / 10 * 4 + 2;
                float waitTime = 0.5f / selectCntStage;
                //clamp 
                waitTime = Mathf.Clamp(waitTime, 0.01f, 0.5f);
                yield return new WaitForSeconds(waitTime); //这样会越来越快
                bool condition = isDiSelect ? (selectCnt > 0) : (selectCnt < upperCnt);
                if (condition)
                {
                    selectCnt += delta;
                    thisSelectCnt++;
                    NewSelect(index, isDiSelect);
                }
            }
        }

        public void NewSelect(int index, bool isDiSelect = false) //最后一个参数表示是否是取消选择
        {
            //这个应该是处理后端逻辑，应该和每个Item的UI表现分开
            if (m_maxChooseCnt == 1)
            {
                Select(index);
                return;
            }

            //传入的index是真实的index
            if (m_Datas == null) return;
            GenshinDemoListData listData = m_Datas[index] as GenshinDemoListData;
            bool multiSelectInOneItem = listData.multiSelectInOneItem;
            int maxCount = listData.count;
            if (m_SelectIndexs.ContainsKey(index)) //在选择的队列当中
            {
                //取消选择
                int checkIndex = index - GetCurrentItemIndex();
                bool addSelect = multiSelectInOneItem && !isDiSelect;
                if (addSelect)
                {
                    AddSelectIndex(index, maxCount,multiSelectInOneItem);
                    RefleshData();
                    if (checkIndex >= 0 && checkIndex < m_LoopItems.Count)
                    {
                        //m_LoopItems[checkIndex].UpdateData(m_Datas, index, localUserData);
                        m_LoopItems[checkIndex].CheckSelect(index, m_Datas[index]);
                    }
                }
                else
                {
                    RemoveSelectIndex(index, multiSelectInOneItem); //这里会顺便UpdateData
                    if (checkIndex >= 0 && checkIndex < m_LoopItems.Count)
                    {
                        m_LoopItems[checkIndex].CheckSelect(index, m_Datas[index], true);
                    }

                }

            }
            else
            {
                //选择
                if (m_SelectIndexs.Count >= m_maxChooseCnt)
                {
                    Debug.Log("选择数量超过最大值");
                    //todo：应该给一个Message提示
                    return;
                }

                AddSelectIndex(index, maxCount, multiSelectInOneItem);
                RefleshData();
                int checkIndex = index - GetCurrentItemIndex();
                if (checkIndex >= 0 && checkIndex < m_LoopItems.Count)
                {
                    m_LoopItems[checkIndex].CheckSelect(index, m_Datas[index]);
                }

            }
        }

        private void RefleshData()
        {
            //遍历字典
            foreach (var item in m_SelectIndexs)
            {
                RefleshSingleData(item.Key);
            }
        }
        
        /// <summary>
        /// 移动
        /// </summary>
        public void MoveTo(int index, float duration = 0.3f)
        {
            //todo:Issue
            //return;
            index = Mathf.Clamp(index, 0, m_Datas.Count - 1);

            int xIndex = 0, yIndex = 0;

            if (m_AxisType == Axis.Horizontal)
            {
                yIndex = index % m_CountOfOtherAxis;
                xIndex = index / m_CountOfOtherAxis;
            }
            else
            {
                xIndex = index % m_CountOfOtherAxis;
                yIndex = index / m_CountOfOtherAxis;
            }

            m_ScrollRect.StopMovement();

            // x
            float pWidth = (m_Content.transform.parent as RectTransform).rect.width;
            float sWidth = m_Content.rect.width;
            float x = m_HorizontalStartSpace + (m_ChildWidth + m_HorizontalSpace) * xIndex;
            float limit = 0;
            if (sWidth > pWidth)
            {
                limit = sWidth - pWidth;
            }

            if (m_AlignType == AlignType.Left)
                x = Mathf.Clamp(-x, -limit, limit);
            else
                x = Mathf.Clamp(x, -limit, limit);

            // y
            float pHeight = (m_Content.transform.parent as RectTransform).rect.height;
            float sHeight = m_Content.rect.height;
            float y = m_VerticalStartSpace + (m_ChildHeight + m_VerticalSpace) * yIndex;
            limit = 0;
            if (sHeight > pHeight)
            {
                limit = sHeight - pHeight;
            }

            if (m_AlignType == AlignType.Top)
                y = Mathf.Clamp(y, -limit, limit);
            else
                y = Mathf.Clamp(-y, -limit, limit);

            if (m_Tweener != null)
            {
                UpdateContent(GetCurrentItemIndex());
                m_Tweener.Kill();
                m_Tweener = null;
            }

            if (duration > 0 && Vector2.Distance(new Vector2(x, y), m_Content.anchoredPosition) > 1f)
            {
                m_Tweener = m_Content.DOAnchorPos(new Vector2(x, y), duration);
            }
            else
            {
                m_Content.anchoredPosition = new Vector2(x, y);
            }

            if (m_Tweener != null)
            {
                m_Tweener.onComplete += () => { UpdateContent(GetCurrentItemIndex()); };
            }
            else
            {
                UpdateContent(GetCurrentItemIndex());
            }
        }

        private int startDragIndexX = -1;
        private int startDragIndexY = -1;
        private bool firstJudgeDirection = true;
        private bool dragBigger = false; //是否初次是在往大的地方drag
        private bool biggerThanStart = false; //是否在往比Start更大的地方drag
        private bool biggerThanLast = false; //跟上次鼠标在的位置比，是否更大
        private bool firstBeingSelected = false;
        private bool lastBiggerThanStart = false; //上次存储的结果，记录是否发生了翻转的情况
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!m_canRangeSelect || startSelectIndex==-1) return;
            Debug.LogError("BeginDrag");
            //startSelectIndex = GetCurrentContentIndex(eventData, out startDragIndexX, out startDragIndexY);
            lastMouseContentIndex = startSelectIndex;
            tmpChooseIndexs.Clear();
            tmpChooseIndexs.Add(startSelectIndex);
            NewSelect(startSelectIndex, false);
            firstJudgeDirection = true;
            //关闭ScrollView的滚动能力
            m_ScrollRect.enabled = false;
        }

        private int GetCurrentContentIndex(PointerEventData eventData, out int xIndex, out int yIndex)
        {
            //找到当前鼠标位置对应content中的哪个item
            //todo:这个getIndex不一定具备可扩展性，需要根据实际情况修改，这里只是一个简单的实现
            int resIndex = -1;
            xIndex = -1;
            yIndex = -1;
            switch (m_AxisType)
            {
                case Axis.Vertical:
                    float y = eventData.position.y - m_Content.position.y;
                    yIndex = Mathf.FloorToInt(y / (m_ChildHeight + m_VerticalSpace));
                    xIndex = Mathf.FloorToInt((eventData.position.x - m_Content.position.x) / (m_ChildWidth + m_HorizontalSpace));
                    yIndex = -yIndex;
                    yIndex -= 1;
                    resIndex = yIndex * m_CountOfOtherAxis + xIndex;
                    //Debug.LogError("xIndex:" + xIndex + " yIndex:" + yIndex);
                    break;
            }

            return resIndex;
        }

        private void AddOrRemoveSelect(int lastMouseContentIndex, int currentMouseContentIndex, bool isAddSelect, bool isAddingStage) //isAddingStage表示是加选阶段还是减选阶段
        {
            int startIndex = Mathf.Min(lastMouseContentIndex, currentMouseContentIndex);
            int endIndex = Mathf.Max(lastMouseContentIndex, currentMouseContentIndex);
            //如果start的值在last和current之间，就更新对应某一侧值为start
            // if(currentMouseContentIndex<startSelectIndex && startSelectIndex<lastMouseContentIndex)
            // {
            //     startIndex = startSelectIndex;
            // }
            // else if(currentMouseContentIndex>startSelectIndex && startSelectIndex>lastMouseContentIndex)
            // {
            //     endIndex = startSelectIndex;
            // }
            
            if (startIndex < 0) startIndex = 0;
            if (isAddSelect)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    tmpChooseIndexs.Add(i);
                    if (!m_SelectIndexs.ContainsKey(i))
                    {
                        NewSelect(i, false);
                    }
                }
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (m_SelectIndexs.ContainsKey(i))
                    {
                        NewSelect(i, true);
                    }
                    tmpChooseIndexs.Remove(i);
                }
            }
        }

        private int lastMouseContentIndex = -1;
        public void OnDrag(PointerEventData eventData)
        {
            Debug.LogWarning(m_Content.anchoredPosition.y);
            if(!m_canRangeSelect) return;
            if (startSelectIndex == -1)
            {
                startSelectIndex = GetCurrentContentIndex(eventData, out startDragIndexX, out startDragIndexY);
            }
            int xIndex = -1, yIndex = -1;
            int currentMouseContentIndex = GetCurrentContentIndex(eventData, out xIndex, out yIndex);
            if(currentMouseContentIndex == lastMouseContentIndex) return;  //防止判断多次
            if (firstJudgeDirection)
            {
                dragBigger = currentMouseContentIndex > lastMouseContentIndex;
                biggerThanStart = dragBigger;
                lastBiggerThanStart = dragBigger;
                //判断加选还是减选对应范围
                AddOrRemoveSelect(lastMouseContentIndex, currentMouseContentIndex, !firstBeingSelected,!firstBeingSelected);
                firstJudgeDirection = false;
            }
                
            //Debug.LogError("currentMouseContentIndex:" + currentMouseContentIndex);
            //Log xIndex and yIndex
            Debug.Log("xIndex:" + xIndex + " yIndex:" + yIndex);
            int startX = Mathf.Min(startDragIndexX, xIndex);
            int startY = Mathf.Min(startDragIndexY, yIndex);
            int endX = Mathf.Max(startDragIndexX, xIndex);
            int endY = Mathf.Max(startDragIndexY, yIndex);
            bool isFlip = false;
            
            biggerThanStart = currentMouseContentIndex >= startSelectIndex;
            biggerThanLast = currentMouseContentIndex > lastMouseContentIndex;
            
            if(biggerThanStart != lastBiggerThanStart)
            {
                //发生了翻转
                isFlip = true;
            }
            lastBiggerThanStart = biggerThanStart;

            bool isAdding = false;
            //设定加选的逻辑
            if (!firstBeingSelected)  //现在是加选逻辑
            {
                if (dragBigger)  //第一次鼠标相对往下移动
                {
                    if (biggerThanStart) //现在是比Start更大的方向
                    {
                        if (biggerThanLast) isAdding = true; //还在往下选
                        else isAdding = false; //往上选，但还是比start大
                    }
                    else  //现在是比Start更小的方向
                    {
                        if (biggerThanLast) isAdding = false; //对应往下选，但在start上面，减选
                        else isAdding = true; //对应网上选，但在start上面，加选
                    }
                }
                else  //第一次鼠标相对往上移动
                {
                    if (biggerThanStart)
                    {
                        if(biggerThanLast) isAdding = true;
                        else isAdding = false;
                    }
                    else
                    {
                        if (biggerThanLast) isAdding = false;
                        else isAdding = true;
                    }
                }
            }
            else  //现在是减选逻辑
            {
                if (dragBigger)  //第一次鼠标相对往下移动
                {
                    if (biggerThanStart)
                    {
                        if (biggerThanLast) isAdding = false;
                        else isAdding = true;
                    }
                    else
                    {
                        if (biggerThanLast) isAdding = true;
                        else isAdding = false;
                    }
                }
                else  //第一次鼠标相对往上移动
                {
                    if (biggerThanStart)
                    {
                        if(biggerThanLast) isAdding = false;
                        else isAdding = true;
                    }
                    else
                    {
                        if (biggerThanLast) isAdding = true;
                        else isAdding = false;
                    }
                }
                
            }
            if (isFlip)  //发生了翻转，处理两段,也就是跟上一段比越过了startPos
            {
                //先处理last到start
                AddOrRemoveSelect(lastMouseContentIndex, startSelectIndex, !isAdding, !firstBeingSelected);
                //再处理start到current
                AddOrRemoveSelect(startSelectIndex, currentMouseContentIndex, isAdding, !firstBeingSelected);    
            }
            else
            {
                AddOrRemoveSelect(lastMouseContentIndex, currentMouseContentIndex, isAdding, !firstBeingSelected);
            }
            //last 和 current之间找到最小，最大值，加选
            
            Debug.LogError("==================================");
            Debug.LogError("第一次是往下选么？" + dragBigger);
            Debug.LogError("现在是在比Start更大的方向么？" + biggerThanStart);
            Debug.LogError("现在是在比Last更大的方向么？" + biggerThanLast);
            Debug.LogError("isAdding:" + isAdding);
            Debug.LogError("StartIndex:" + startSelectIndex + " CurrentIndex:" + currentMouseContentIndex);
            Debug.LogError("==================================");
            
            
            //bool isAdding = tmpChooseIndexs.Count > 0;
            //tmpChooseIndexs.Clear();
            int startIdx = startY * m_CountOfOtherAxis + startX;
            int endIdx = endY * m_CountOfOtherAxis + endX;
            // for (int i = startIdx; i <= endIdx; i++)
            // {
            //     tmpChooseIndexs.Add(i);
            //     //Debug.LogWarning(i);
            // }
            // //Debug.LogError("startIdx:" + startIdx + " endIdx:" + endIdx);
            //
            // int leftUpCornerIndex = GetCurrentItemIndex();
            // for(int i = 0; i < m_LoopItems.Count; i++)
            // {
            //     int tmpIndex = leftUpCornerIndex + i;
            //     if (tmpChooseIndexs.Contains(tmpIndex))
            //     {
            //         ForceSetSelectedIndex(tmpIndex, true);
            //     }
            //     else
            //     {
            //         ForceSetSelectedIndex(tmpIndex, false);
            //     }
            // }
            //RefleshData();
            
            
            
            //跟上次记录的值比，此时正在往右或往下走是加选，往左或者往上走是减选
            // if (currentMouseContentIndex >= lastMouseContentIndex)
            // {
            //     //大于lastCurrent，小于current的的全部加选
            //     for (int i = lastMouseContentIndex + 1; i <= currentMouseContentIndex; i++)
            //     {
            //         if (!tmpChooseIndexs.Contains(i))
            //         {
            //             tmpChooseIndexs.Add(i);
            //             NewSelect(i, false);
            //         }
            //     }
            // }
            // else
            // {
            //     //此时在往左上角走，说明是减选，那么就遍历减选的index，删除
            //     //lastMouseContentIndex 之后的全部删除
            //     for (int i = tmpChooseIndexs.Count - 1; i >= 0; i--)
            //     {
            //         if (tmpChooseIndexs[i] > currentMouseContentIndex)
            //         {
            //             NewSelect(tmpChooseIndexs[i], true);
            //             tmpChooseIndexs.RemoveAt(i);
            //         }
            //     }
            // }

            lastMouseContentIndex = currentMouseContentIndex;

            //int index = GetCurrentItemIndex();
            //到当前页面的底部了
            // if(yIndex >= m_VerticalCount - 1 || yIndex <= 0)
            // {
            //     //移动一行
            //     MoveTo(index);
                    //先不move了，效果不太好，todo：暂时不太会做，先这样
            // }
        }
        
        //强制设值，index是真实的index值
        private void ForceSetSelectedIndex(int index, bool isSelect)
        {
            if (isSelect)
            {
                if (m_SelectIndexs.ContainsKey(index))
                {
                    m_SelectIndexs[index] = 1;
                    int  checkIndex = index - GetCurrentItemIndex();
                    RefleshData();
                    if (checkIndex >= 0 && checkIndex < m_LoopItems.Count)
                    {
                        //m_LoopItems[checkIndex].UpdateData(m_Datas, index, localUserData);
                        m_LoopItems[checkIndex].CheckSelect(index, m_Datas[index]);
                    }
                }
                else
                {
                    m_SelectIndexs.Add(index, 1);
                    int checkIndex = index - GetCurrentItemIndex();
                    RefleshData();
                    if (checkIndex >= 0 && checkIndex < m_LoopItems.Count)
                    {
                        //m_LoopItems[checkIndex].UpdateData(m_Datas, index, localUserData);
                        m_LoopItems[checkIndex].CheckSelect(index, m_Datas[index]);
                    }
                }
            }
            else
            {
                if (m_SelectIndexs.ContainsKey(index))
                {
                    m_SelectIndexs.Remove(index);
                    int checkIndex = index - GetCurrentItemIndex();
                    RefleshData();
                    if (checkIndex >= 0 && checkIndex < m_LoopItems.Count)
                    {
                        //m_LoopItems[checkIndex].UpdateData(m_Datas, index, localUserData);
                        m_LoopItems[checkIndex].CheckSelect(index, m_Datas[index], true);
                    }
                }
            }
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //if (!m_IsPaging) return;
            // 计算最近的一页 并设置
            int index = GetCurrentItemIndex();
            Debug.Log("onEndDrag index:" + index);
            m_ScrollRect.enabled = true;
            firstJudgeDirection = true;
            dragBigger = false;
            biggerThanStart = false;
            biggerThanLast = false;
            firstBeingSelected = false;
            tmpChooseIndexs.Clear();
            startSelectIndex = -1;
            //MoveTo(index);
        }

        private void Update()
        {
            if (m_Datas == null || m_Datas.Count == 0 || m_PrefabPool == null || m_ItemType == null) return;

            var parent = (m_ScrollRect.transform as RectTransform);
            if (parentRect != parent.rect)
            {
                SetUpList(m_Datas, m_PrefabPool.Prefab, m_ItemType, true, m_UserData);
            }
        }

        public void Release()
        {
            m_LoopItems.Clear();
            if (m_PrefabPool != null)
                m_PrefabPool.RecycleUseList();
        }
    }
}
