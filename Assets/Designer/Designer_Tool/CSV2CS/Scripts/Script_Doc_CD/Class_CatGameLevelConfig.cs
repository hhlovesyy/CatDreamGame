using UnityEngine; 
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
	}
	