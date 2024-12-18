﻿using UnityEngine; 
using System.Collections; 
public class Class_CatGameLevelConfig { 
	public string levelID { get; set; }    //关卡编号
	public int _levelID (){
		int value = int.Parse(levelID);
		return value;
	}
	public string levelDescription { get; set; }    //关卡简介
	  public string _levelDescription (){
		string value = levelDescription;
		return value;
	}
	public string levelIconAddressable { get; set; }    //关卡图像资源链接
	  public string _levelIconAddressable (){
		string value = levelIconAddressable;
		return value;
	}
	public string levelTitle { get; set; }    //关卡标题
	  public string _levelTitle (){
		string value = levelTitle;
		return value;
	}
	public string levelTotalTime { get; set; }    //关卡允许的最高时长
	public int _levelTotalTime (){
		int value = int.Parse(levelTotalTime);
		return value;
	}
	public string levelPosition { get; set; }    //关卡场景的初始坐标
	  public string _levelPosition (){
		string value = levelPosition;
		return value;
	}
	public string LevelLoseTip { get; set; }    //关卡失败的祝福语
	  public string _LevelLoseTip (){
		string value = LevelLoseTip;
		return value;
	}
	public string LevelDifficulty { get; set; }    //关卡难度
	public int _LevelDifficulty (){
		int value = int.Parse(LevelDifficulty);
		return value;
	}
	}
	