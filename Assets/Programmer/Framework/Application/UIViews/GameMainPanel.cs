using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace OurGameFramework
{
    public class GameMainPanel : UIView
    {
        #region 控件绑定变量声明，自动生成请勿手改
#pragma warning disable 0649
        [ControlBinding]
        public Slider SliderCommon1;
        [ControlBinding]
        public RectTransform HurtShowTexts;
        [ControlBinding]
        public RawImage PawSkillIcon;
        [ControlBinding]
        public TextMeshProUGUI RemainTime;
        [ControlBinding]
        public Slider SliderCommon2;

#pragma warning restore 0649
        #endregion
        
        private SliderController sliderController;
        private Coroutine timeCountDown;
        private int remainTime = 0;
        private int totalAllowTime = 0;
        private int bestUseTime = 0;
        private int levelID = -1;
        
        private Sequence skillShowSequence;
        private int showHurtIndex = 0;
        private int maxHurtShowCount = 5;
        //记录HurtShowTexts所有子物体的初始位置
        private List<Vector3> hurtShowTextsInitPos = new List<Vector3>();
        private List<TMP_Text> hurtShowTexts = new List<TMP_Text>();

        public override void OnInit(UIControlData uIControlData, UIViewController controller)
        {
            base.OnInit(uIControlData, controller);
            EventManager.AddEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(), NoticeSliderValueChange);
            EventManager.AddEvent<GameStatusEvent>(GameEvent.CHANGE_GAME_STATUS.ToString(), GameOver);
            EventManager.AddEvent<CatSkillStatusEvent, float>(GameEvent.CHANGE_SKILL_STATUS.ToString(), CatSkillStatusChange);
        }
        
        private void CatSkillStatusChange(CatSkillStatusEvent catSkillStatusEvent, float remainTime)
        {
            if (catSkillStatusEvent == CatSkillStatusEvent.RELEASE_SKILL)
            {
                //激活技能
                PawSkillIcon.transform.localScale = Vector3.one;
                PawSkillIcon.color = Color.grey;
                CanvasGroup canvasGroup = PawSkillIcon.GetComponent<CanvasGroup>();
                if(skillShowSequence!=null) skillShowSequence.Kill();
                skillShowSequence = DOTween.Sequence();
                skillShowSequence.Append(PawSkillIcon.transform.DOShakeScale(0.3f));
                skillShowSequence.Append(canvasGroup.DOFade(0f, 0.01f).OnComplete(() =>
                {
                    //技能放完了，淡出到0，然后逐渐淡入
                    canvasGroup.DOFade(1f, remainTime - 0.3f); //逐渐淡出
                }));
                //sequence 播放
                skillShowSequence.Play();
            }
            else if (catSkillStatusEvent == CatSkillStatusEvent.SKILL_RESUME)
            {
                PawSkillIcon.transform.localScale = Vector3.one;
                //技能恢复
                PawSkillIcon.color = Color.white;
                PawSkillIcon.transform.DOShakeScale(0.3f);
            }
        }

        private void SaveResultToXml(int useTime, int bestTime, int starLevel)
        {
            int levelID = this.levelID;
            if (useTime < bestTime || bestTime == -1)
            {
                bestTime = useTime;
            }
            CatGameXMLReader.Instance.SavePlayerData(levelID,bestTime,starLevel);
            HGameRoot.Instance.playerData = CatGameXMLReader.Instance.ReadPlayerData();
        }

        private int GetStarLevel()
        {
            //通过即为1星；2星就3分钟；3星就1分钟。先暂定这个吧，之后做出来在调整~
            float remainRatio = (float)remainTime / totalAllowTime;
            if (remainRatio >= 0.8)
            {
                return 3;
            }
            else if (remainRatio >= 0.4)
            {
                return 2;
            }
            else return 1;
        }

        private void GameOver(GameStatusEvent gameStatus)
        {
            Debug.Log("gameOver");
            if (gameStatus == GameStatusEvent.GAME_WIN)
            {
                StopCoroutine(timeCountDown);
                Debug.Log("You win this game!!");
                int starLevel = GetStarLevel();
                int useTime = totalAllowTime - remainTime;
                int bestTime = this.bestUseTime;
                SaveResultToXml(useTime, bestTime, starLevel);
                GameOverStruct gameOverStruct = new GameOverStruct();
                gameOverStruct.starLevel = starLevel;
                gameOverStruct.totalUseTime = useTime;
                gameOverStruct.bestUseTime = bestTime;
                gameOverStruct.levelID = levelID;
                UIManager.Instance.Open(UIType.LevelWinPanelView, gameOverStruct);
            }
            else if(gameStatus == GameStatusEvent.GAME_LOSE)
            {
                Debug.Log("You lose this game!!");
                GameOverStruct gameOverStruct = new GameOverStruct();
                gameOverStruct.levelID = levelID;
                UIManager.Instance.Open(UIType.LevelLosePanelView, gameOverStruct);
            }
        }
        
        private void ShowHurtValue(float addValue)
        {
            if (showHurtIndex >= maxHurtShowCount)
            {
                showHurtIndex = 0;
            }
            TMP_Text hurtText = hurtShowTexts[showHurtIndex];
            hurtText.text = addValue.ToString();
            showHurtIndex++;
            //伤害显示的动画,dotween, 往上移动，结束后回到原位，设置为0
            hurtText.transform.DOMoveY(hurtText.transform.position.y + 50, 1f).OnComplete(() =>
            {
                hurtText.transform.position = hurtShowTextsInitPos[showHurtIndex - 1];
                hurtText.text = "";
            });
        }   
        
        private void NoticeSliderValueChange(SliderEvent sliderEvent, string sliderName, float addValue)
        {
            //Debug.LogError("now we are here");
            if (sliderController)
            {
                if (sliderEvent == SliderEvent.SLIDER_VALUE_CHANGE)
                {
                    sliderController.ChangeSliderValue(sliderName, addValue, true); //中间过程用tween，直接的slider值其实不用tween
                    // 显示伤害
                    ShowHurtValue(addValue);
                }
                else if(sliderEvent == SliderEvent.SLIDER_UPPERBOUND_CHANGE)
                    sliderController.ChangeSliderUpperBound(sliderName, addValue);
            }
        }

        private void ResetPawUI()
        {
            if(skillShowSequence!=null) skillShowSequence.Kill();
            PawSkillIcon.color = Color.white;
            PawSkillIcon.transform.localScale = Vector3.one;
        }
        private void ResetUI()
        {
            ResetPawUI();
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
        
        IEnumerator TimeCountDown(int totalAllowTime)
        {
            remainTime = totalAllowTime;
            while (remainTime > 0)
            {
                yield return new WaitForSeconds(1f);
                remainTime = remainTime - 1; //剩余的时间
                RemainTime.text = FormatIntTimeToString(remainTime);
                //每30s 闪烁一次，或者最后20s也闪烁
                if (remainTime % 30 == 0 || remainTime <= 20)
                {
                    RemainTime.transform.DOShakeScale(0.3f);
                    RemainTime.color = Color.red;
                }
                else
                {
                    RemainTime.color = Color.white;
                }
            }
            GameOver(GameStatusEvent.GAME_LOSE);
        }
        
        private void BeginGame(int levelID, int totalAllowTime)
        {
            if(timeCountDown!=null) StopCoroutine(timeCountDown);
            timeCountDown = StartCoroutine(TimeCountDown(totalAllowTime));
            this.totalAllowTime = totalAllowTime;
            RemainTime.text = FormatIntTimeToString(totalAllowTime);
            //开始放音乐
            HAudioManager.Instance.Play("GameMainMusic", HGameRoot.Instance.gameObject);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ResetUI();
            Time.timeScale = 1f;
            GameMainPanelStruct gameMainPanelStruct = userData as GameMainPanelStruct;
            if (gameMainPanelStruct == null)
            {
                Debug.LogWarning("GameMainPanelStruct is null");
                return;
            }
            levelID = gameMainPanelStruct.levelID;
            int totalAllowTime = gameMainPanelStruct.totalAllowTime;
            bestUseTime = gameMainPanelStruct.bestUseTime;
            maxHurtShowCount = HurtShowTexts.childCount;
            BeginGame(levelID, totalAllowTime);
            //初始化游戏设置
            GameObject gameRoot = HGameRoot.Instance.gameObject;
            sliderController = gameRoot.GetOrAddComponent<SliderController>();
            sliderController.SetSliders(new List<Slider> { SliderCommon1 }, levelID);
            
            //初始化伤害显示
            for (int i = 0; i < maxHurtShowCount; i++)
            {
                TMP_Text hurtText = HurtShowTexts.GetChild(i).GetComponent<TMP_Text>();
                hurtShowTexts.Add(hurtText);
                hurtShowTextsInitPos.Add(hurtText.transform.position);
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
            //清空slider条
            if (sliderController)
            {
                sliderController.ClearSliders();
            }
        }
    }
}
