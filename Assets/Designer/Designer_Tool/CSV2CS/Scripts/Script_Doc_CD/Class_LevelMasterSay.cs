using UnityEngine; 
using System.Collections; 
public class Class_LevelMasterSay { 
	public string sayWorldId { get; set; }    //说话语句id
	public int _sayWorldId (){
		int value = int.Parse(sayWorldId);
		return value;
	}
	public string worldAppearLevel { get; set; }    //出现的关卡
	public int _worldAppearLevel (){
		int value = int.Parse(worldAppearLevel);
		return value;
	}
	public string sayContent { get; set; }    //说话内容
	  public string _sayContent (){
		string value = sayContent;
		return value;
	}
	}
	