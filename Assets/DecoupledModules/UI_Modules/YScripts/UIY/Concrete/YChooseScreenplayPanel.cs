using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YChooseScreenplayPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/YChooseScreenplayPanel";
    public YChooseScreenplayPanel() : base(new UIType(path)){}
    TMP_Dropdown dropdownCha;
    TMP_Dropdown dropdownAnim;
    TMP_Dropdown dropdownAudio;
    TMP_Dropdown dropdownOrigin;
    TMP_Dropdown dropdownDestination;
    
    List<string> dropdownChaOptions = new List<string>(){"Character1","Character2","Character3"};
    List<string> dropdownAnimOptions = new List<string>(){"animation1","animation2","animation3"};
    List<string> dropdownAudioOptions = new List<string>(){"audio1","audio2","audio3"};
    List<string> dropdownOriginOptions = new List<string>(){"origin1","origin2","origin3"};
    List<string> dropdownDestinationOptions = new List<string>(){"destination1","destination2","destination3"};
    
    public override void OnEnter()
    {
        uiTool.GetOrAddComponentInChilden<Button>("OkButton").onClick.AddListener(()=>
        {
            //Debug.Log("点击了开始按钮");
            //YGameRoot.Instance.SceneSystem.SetScene(new YMainScene());
            Pop();
            Push(new YMainPanel());
            
        });
        uiTool.GetOrAddComponentInChilden<Button>("BackButton").onClick.AddListener(()=>
        {
            Pop();
            Push(new StartPanel());
        });
        
        dropdownCha = uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownCha");
        dropdownAnim = uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownAnim");
        dropdownAudio = uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownAudio");
        dropdownOrigin = uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownOrigin");
        dropdownDestination = uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownDestination");
        
        dropdownCha.AddOptions(dropdownChaOptions);
        dropdownAnim.AddOptions(dropdownAnimOptions);
        dropdownAudio.AddOptions(dropdownAudioOptions);
        dropdownOrigin.AddOptions(dropdownOriginOptions);
        dropdownDestination.AddOptions(dropdownDestinationOptions);
        
        
        dropdownCha.onValueChanged.AddListener((int value)=>
        {
            Debug.Log("DropdownCha选择了" + value);
        });
        dropdownAnim.onValueChanged.AddListener((int value)=>
        {
            Debug.Log("DropdownAnim选择了" + value);
        });
        dropdownAudio.onValueChanged.AddListener((int value)=>
        {
            Debug.Log("DropdownAudio选择了" + value);
        });
        dropdownOrigin.onValueChanged.AddListener((int value)=>
        {
            Debug.Log("DropdownOrigin选择了" + value);
        });
        dropdownDestination.onValueChanged.AddListener((int value)=>
        {
            Debug.Log("DropdownDestination选择了" + value);
        });
        //以下是下拉框的监听
        //DropdownCha
        // uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownCha").onValueChanged.AddListener((int value)=>
        // {
        //     Debug.Log("DropdownCha选择了" + value);
        // });
        // //DropdownAnim
        // uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownAnim").onValueChanged.AddListener((int value)=>
        // {
        //     Debug.Log("DropdownAnim选择了" + value);
        // });
        // //DropdownAudio
        // uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownAudio").onValueChanged.AddListener((int value)=>
        // {
        //     Debug.Log("DropdownAudio选择了" + value);
        // });
        // //DropdownOrigin
        // uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownOrigin").onValueChanged.AddListener((int value)=>
        // {
        //     Debug.Log("DropdownOrigin选择了" + value);
        // });
        // //DropdownDestination
        // uiTool.GetOrAddComponentInChilden<TMP_Dropdown>("DropdownDestination").onValueChanged.AddListener((int value)=>
        // {
        //     Debug.Log("DropdownDestination选择了" + value);
        // });
        
    }
}
