using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MessageBoxBaseStruct
{
    private string messageId;
    private string messageContent;
    private int messageType;
    private float messageShowTime;
    private string messageTransitionEffect;
    private string messageLink;
    
    #region Gets And Sets
    
    public string MessageLink
    {
        get { return messageLink; }
    }
    public string MessageId
    {
        get { return messageId; }
    }
    
    public string MessageContent
    {
        get { return messageContent; }
    }
    
    public int MessageType
    {
        get { return messageType; }
    }
    
    public float MessageShowTime
    {
        get { return messageShowTime; }
    }
    
    public string MessageTransitionEffect
    {
        get { return messageTransitionEffect; }
    }
    
    #endregion
    
    public MessageBoxBaseStruct(string id, string content, int type, float showTime, string transitionEffect, string link)
    {
        messageId = id;
        messageContent = content;
        messageType = type;
        messageShowTime = showTime;
        messageTransitionEffect = transitionEffect;
        messageLink = link;
    }

    public void SetMessage(string message)
    {
        messageContent = new string(message);
    }
}

public class yPlanningTable : MonoBehaviour
{
    
    //静态类
    public static yPlanningTable Instance{get;private set;}
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        ReadAllMessages();
    }

    private Dictionary<string, MessageBoxBaseStruct> messages = new Dictionary<string, MessageBoxBaseStruct>();
    public Dictionary<string, MessageBoxBaseStruct> Messages
    {
        get { return messages; }
    }
    void ReadAllMessages()
    {
        string messageLink = "Assets/Designer/DesignerTableCommon/MessageCommonCSVFile.csv";
        string[] fileData = File.ReadAllLines(messageLink);
        for (int i = 3; i < fileData.Length; i++)
        {
            string[] rowData = fileData[i].Split(',');
            string messageId = rowData[0];
            int messageKind = int.Parse(rowData[1]);
            string messageContent = rowData[2];
            float messageShowTime = float.Parse(rowData[3]);
            string messageTransitionEffect = rowData[4];
            string messagePrefabLink = rowData[5];
            MessageBoxBaseStruct aMessage = new MessageBoxBaseStruct(messageId, messageContent,messageKind,messageShowTime,messageTransitionEffect, messagePrefabLink);
            messages.Add(messageId, aMessage);
        }
        Debug.Log("finish reading messages!");
    }
    
}