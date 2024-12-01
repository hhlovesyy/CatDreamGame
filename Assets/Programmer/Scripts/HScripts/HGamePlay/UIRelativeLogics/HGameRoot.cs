using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OurGameFramework;
using UnityEngine.InputSystem;

//enum SliderEvent
public enum SliderEvent 
{
    SLIDER_VALUE_CHANGE, //slider的事件
    SLIDER_UPPERBOUND_CHANGE,
}

public enum KeyDownEvent
{
    KEY_H,
}

public enum GameStatusEvent
{
    GAME_START,
    GAME_WIN,
    GAME_LOSE,
}

public enum CatSkillStatusEvent
{
    RELEASE_SKILL,  //释放技能
    SKILL_RESUME,   //技能恢复
}

public enum GameEvent
{
    CHANGE_SLIDER_VALUE, //改变slider值
    CHANGE_GAME_STATUS, //改变游戏状态
    CHANGE_SKILL_STATUS, //改变技能状态
    KEY_DOWN_GAME_EVENT, //按键事件
}

public enum CollisionSourceType
{
    BODY,
    PAW,
    GROUND,
}

public enum CollisionResultType
{
    BROKEN,
    NONE,
    COMMONFORCE,
}

public class CollisionInfo
{
    public CollisionSourceType collisionSourceType;
    public Vector3 collisionPoint;
    public Vector3 collisionForce;
    public Vector3 collisionVelocity;
}

public class HGameRoot : SingletonMono<HGameRoot>
{
    private float volumeMultiplier = 1f;
    private float mouseSensitive = 1f;  //记录一下，方便下次打开设置界面的时候获取到对应的值
    private bool openPause = false;
    public bool gameStart = false;
    public CatGamePlayerData playerData;

    public int currentMaxLevel = 1;
    public bool hasReadTutorial = false;

    private L2PlayerInput playerInput;

    private GameObject currentPlayer;
    private Vector3 levelStartPos;
    
    public float VolumeMultiplier
    {
        get => volumeMultiplier;
        set
        {
            volumeMultiplier = value;
        }
    }
    
    public float MouseSensitive
    {
        get => mouseSensitive;
        set
        {
            mouseSensitive = value;
        }
    }
    
    public bool OpenPause
    {
        get => openPause;
        set
        {
            openPause = value;
        }
    }

    private void Awake()
    {
        playerInput = new L2PlayerInput();
        playerInput.ShortcutKey.GetOutPausePanel.performed += PressEscape;
        
        playerInput.ShortcutKey.GetOutTutorial.performed += PressTutorial;
        
        playerInput.ShortcutKey.TeleportKey.performed += Teleport;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    
    private void OnDisable()
    {
        playerInput.Disable();
    }

    //回调函数
    private void PressEscape(InputAction.CallbackContext context)
    {
        if (openPause || !gameStart) return;
        openPause = true;
        OurGameFramework.UIManager.Instance.Open(OurGameFramework.UIType.PausePanelView);
    }

    private void PressTutorial(InputAction.CallbackContext context)
    {
        EventManager.DispatchEvent<KeyDownEvent>(GameEvent.KEY_DOWN_GAME_EVENT.ToString(), KeyDownEvent.KEY_H);
    }
    
    //传送到当前房间的正中心，也就是初始出生位置
    private void Teleport(InputAction.CallbackContext context)
    {
        if (!gameStart) return;
        Debug.Log("now in teleport function");
        //将玩家传送到本关初始的位置
        if (currentPlayer != null && levelStartPos != null)
        {
            OurGameFramework.UIManager.Instance.Open(OurGameFramework.UIType.UIMessageBoxView,
                ObjectPool<MessageBoxData>.Get().Set("提示", "有时动不了的话，尝试跳一下或许就能解决问题喵！<color=#FF0000><size=120%>是否仍要确认脱离卡死？</size></color>", () =>
                {
                    ConfirmTeleport();
                    Debug.Log("confirmTeleport");
                }, () =>
                {
                    OurGameFramework.UIManager.Instance.Close(OurGameFramework.UIType.UIMessageBoxView);
                }));
        }
    }

    private void ConfirmTeleport()
    {
        CharacterController characterController = currentPlayer.GetComponent<CharacterController>();
        characterController.enabled = false;
        currentPlayer.transform.position = levelStartPos;
        characterController.enabled = true;
    }

    public void SetPlayerBaseInfo(GameObject player)
    {
        currentPlayer = player;
        levelStartPos = player.transform.position;
    }
    

    private void OnGUI()
    {
        //just test button
        // if (GUI.Button(new Rect(10, 10, 100, 50), "ChangeSlider1"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", -500f);
        // }
        //
        // if (GUI.Button(new Rect(10, 70, 100, 50), "ResetStorage"))
        // {
        //     // EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //     //     SliderEvent.SLIDER_VALUE_CHANGE, "Slider1", -10f);
        //     CatGameXMLReader.Instance.ResetPlayerData();
        // }
        // //
        // if (GUI.Button(new Rect(10, 130, 100, 50), "UseSkill"))
        // {
        //     EventManager.DispatchEvent<CatSkillStatusEvent, float>(GameEvent.CHANGE_SKILL_STATUS.ToString(), CatSkillStatusEvent.RELEASE_SKILL, 5f);
        // }
        //
        // if (GUI.Button(new Rect(10, 190, 100, 50), "SkillResume"))
        // {
        //     EventManager.DispatchEvent<CatSkillStatusEvent, float>(GameEvent.CHANGE_SKILL_STATUS.ToString(), CatSkillStatusEvent.SKILL_RESUME, 5f);
        // }
        //
        // if (GUI.Button(new Rect(10, 190, 100, 50), "ChangeSlider2_minus"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_VALUE_CHANGE, "Slider2", -5f);
        // }
        //
        // if (GUI.Button(new Rect(10, 250, 100, 50), "ChangeSlider1UpperBound"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider1", 20f);
        // }
        //
        // if (GUI.Button(new Rect(10, 310, 100, 50), "ChangeSlider2UpperBound"))
        // {
        //     EventManager.DispatchEvent<SliderEvent, string, float>(GameEvent.CHANGE_SLIDER_VALUE.ToString(),
        //         SliderEvent.SLIDER_UPPERBOUND_CHANGE, "Slider1", -10);
        // }
    }
}
