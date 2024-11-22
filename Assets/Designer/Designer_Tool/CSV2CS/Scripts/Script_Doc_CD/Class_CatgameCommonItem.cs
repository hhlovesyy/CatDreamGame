﻿using UnityEngine; 
using System.Collections; 
public class Class_CatgameCommonItem { 
	public string ItemID { get; set; }    //物品ID
	public int _ItemID (){
		int value = int.Parse(ItemID);
		return value;
	}
	public string ItemName { get; set; }    //物品名称
	  public string _ItemName (){
		string value = ItemName;
		return value;
	}
	public string ItemDescription { get; set; }    //物品描述
	  public string _ItemDescription (){
		string value = ItemDescription;
		return value;
	}
	public string ItemSliderEffect { get; set; }    //物品对不同条的影响值，用分号隔开
	  public string _ItemSliderEffect (){
		string value = ItemSliderEffect;
		return value;
	}
	public string ItemSliderUpperBoundEffect { get; set; }    //物品对不同条上限的影响值，-1表示不影响
	  public string _ItemSliderUpperBoundEffect (){
		string value = ItemSliderUpperBoundEffect;
		return value;
	}
	public string ItemDropGroundSound { get; set; }    //物品的落地音效
	  public string _ItemDropGroundSound (){
		string value = ItemDropGroundSound;
		return value;
	}
	public string ItemWeight { get; set; }    //物体重量
	public int _ItemWeight (){
		int value = int.Parse(ItemWeight);
		return value;
	}
	}
	