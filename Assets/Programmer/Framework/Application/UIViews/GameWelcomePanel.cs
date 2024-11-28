using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace OurGameFramework
{
    public class GameWelcomePanel : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        public Button StartGameBotton;
        [ControlBinding]
        public RectTransform TweenYinfu;
        [ControlBinding]
        public Button SettingButton;
        [ControlBinding]
        public Button ExitButton;

#pragma warning restore 0649
        #endregion
        
        private List<Tween> yinfuTweens = new List<Tween>();
        
        private void StartGame()
        {
            Debug.Log("Start Game!");
            HGameRoot.Instance.gameStart = true;
            //UIManager.Instance.Open(UIType.GameMainPanel, gameMainPanelStruct);
            //UIManager.Instance.Preload(UIType.LevelChoosePanelView);
            UIManager.Instance.Open(UIType.LevelChoosePanelView, HGameRoot.Instance.playerData);
        }

        private void OpenSettings()
        {
            Debug.Log("Open Settings!");
            EventSystem.current.SetSelectedGameObject(null);  //note：这句话比较关键，解决了打开设置界面并返回后，设置按钮默认被按下的问题
            UIManager.Instance.Open(UIType.GameSettingView);
        }

        private void ExitGame()
        {
            Debug.Log("Exit Game!");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            StartGameBotton.onClick.AddListener(StartGame);
            SettingButton.onClick.AddListener(OpenSettings);
            ExitButton.onClick.AddListener(ExitGame);
        }

        private void DoYinfuTween()
        {
            // 音符有移动，有缩放，有透明度变化,具备一定的随机性
            for (int i = 0; i < TweenYinfu.childCount; i++)
            {
                RectTransform yinfu = TweenYinfu.GetChild(i).GetComponent<RectTransform>();

                // 随机初始化位置、缩放和透明度
                Vector3 targetPosition = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), 0);
                float moveDuration = Random.Range(3f, 5f);

                // 初始化移动的Tween
                var moveTween = yinfu.DOLocalMove(targetPosition, moveDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);

                // 使用局部变量保存moveTween，用于在OnStepComplete中访问
                moveTween.OnStepComplete(() =>
                {
                    Vector3 currentPos = yinfu.localPosition;
                    targetPosition = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), 0);
                    //等待两秒
                    moveTween.ChangeStartValue(currentPos); // 更新起始位置
                    moveTween.ChangeEndValue(targetPosition); // 更新目标位置
                });

                // 随机初始化缩放
                Vector3 targetScale = Vector3.one * UnityEngine.Random.Range(0.5f, 1.5f);
                float scaleDuration = Random.Range(1f, 2f);

                // 初始化缩放的Tween
                var scaleTween = yinfu.DOScale(targetScale, scaleDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);

                // 使用局部变量保存scaleTween，用于在OnStepComplete中访问
                scaleTween.OnStepComplete(() =>
                {
                    Vector3 currentScale = yinfu.localScale;
                    targetScale = Vector3.one * UnityEngine.Random.Range(0.5f, 1.5f);
                    //scaleTween.ChangeEndValue(targetScale); // 更新目标缩放
                    scaleTween.ChangeStartValue(currentScale); // 更新起始缩放
                    scaleTween.ChangeEndValue(targetScale); // 更新目标缩放
                });

                // 随机初始化透明度
                float targetAlpha = 1f;
                float fadeDuration = Random.Range(1f, 2f);

                // 初始化透明度的Tween
                var fadeTween = yinfu.GetComponent<CanvasGroup>().DOFade(targetAlpha, fadeDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear);

                // 使用局部变量保存fadeTween，用于在OnStepComplete中访问
                fadeTween.OnStepComplete(() =>
                {
                    float thisAlpha = yinfu.GetComponent<CanvasGroup>().alpha;
                    targetAlpha = thisAlpha > 0.9f ? 0f : 1f;
                    fadeTween.ChangeStartValue(thisAlpha); // 更新起始透明度
                    fadeTween.ChangeEndValue(targetAlpha); // 更新目标透明度
                    //fadeTween.ChangeEndValue(targetAlpha); // 更新目标透明度
                });

                // 添加到yinfuTweens列表（如果需要的话）
                yinfuTweens.Add(moveTween);
                yinfuTweens.Add(scaleTween);
                yinfuTweens.Add(fadeTween);
            }
        }


        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            for(int i=0;i<yinfuTweens.Count;i++)
            {
                yinfuTweens[i].Kill();
            }
            yinfuTweens.Clear();
            DoYinfuTween();
            HAudioManager.Instance.Play("WelcomePanelMusic",HGameRoot.Instance.gameObject);
        }

        public override void OnAddListener()
        {
            base.OnAddListener();
        }

        public override void OnRemoveListener()
        {
            base.OnRemoveListener();
            // StartGameBotton.onClick.RemoveListener(StartGame);
            // SettingButton.onClick.RemoveListener(OpenSettings);
            // ExitButton.onClick.RemoveListener(ExitGame);
        }

        public override void OnClose()
        {
            base.OnClose();
        }
    }
}
