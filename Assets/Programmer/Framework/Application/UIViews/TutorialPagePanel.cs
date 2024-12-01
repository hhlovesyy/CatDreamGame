using System;
using System.Collections;
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
        public Button CloseBtn;
        [ControlBinding]
        public TextMeshProUGUI TutorialText;
        [ControlBinding]
        public RectTransform TutorialPages;
        [ControlBinding]
        public TextMeshProUGUI PageInfo;
        [ControlBinding]
        public Button PreviousPageBtn;
        [ControlBinding]
        public Button NextPageBtn;

#pragma warning restore 0649
        #endregion
        
        private int totalPageCnt = 2;
        private int currentPage = 1;
        private Coroutine tutorialTextCoroutine;
        
        private void CloseTutorial()
        {
            HAudioManager.Instance.Play("ButtonClickAudio", Camera.main.gameObject);
            UIManager.Instance.Close(UIType.TutorialPagePanel);
        }

        private void OpenPreviousPage()
        {
            HAudioManager.Instance.Play("ButtonClickAudio", Camera.main.gameObject);
            if (tutorialTextCoroutine != null)
            {
                StopCoroutine(tutorialTextCoroutine);
                tutorialTextCoroutine = null;
                TutorialText.text = "";
            }
            currentPage--;
            if (currentPage < 1)
            {
                currentPage = totalPageCnt;
            }
            SetPage(currentPage);
            PageInfo.text = currentPage + "/" + totalPageCnt;
        }
        
        private void OpenNextPage()
        {
            HAudioManager.Instance.Play("ButtonClickAudio", Camera.main.gameObject);
            if (tutorialTextCoroutine != null)
            {
                StopCoroutine(tutorialTextCoroutine);
                tutorialTextCoroutine = null;
                TutorialText.text = "";
            }
            currentPage++;
            if (currentPage > totalPageCnt)
            {
                currentPage = 1;
            }
            PageInfo.text = currentPage + "/" + totalPageCnt;
            SetPage(currentPage);
        }
        
        

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            CloseBtn.onClick.AddListener(CloseTutorial);
            PreviousPageBtn.onClick.AddListener(OpenPreviousPage);
            NextPageBtn.onClick.AddListener(OpenNextPage);
            totalPageCnt = TutorialPages.childCount;
            currentPage = 1;
            PageInfo.text = currentPage + "/" + totalPageCnt;
            SetPage(currentPage);
        }

        IEnumerator ShowTutorialText()
        {
            //一个一个字显示
            string text = "糟糕！！！马上就要到点了，主人怎么还在做梦啊！今天可是他的人生大事，千万不能睡过头呀！快，想想办法，充分利用房间里的各种东西，搞搞破坏，闹闹动静把他吵醒吧！";
            for (int i = 0; i < text.Length; i++)
            {
                TutorialText.text = text.Substring(0, i + 1);
                yield return new WaitForSeconds(0.02f);
            }
        }

        private void SetPage(int index)
        {
            for(int i = 0; i < TutorialPages.childCount; i++)
            {
                TutorialPages.GetChild(i).gameObject.SetActive(i == index - 1);
            }

            if (index == 1)
            {
                if (tutorialTextCoroutine != null)
                {
                    StopCoroutine(tutorialTextCoroutine);
                    tutorialTextCoroutine = null;
                }
                tutorialTextCoroutine = StartCoroutine(ShowTutorialText());
            }
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
