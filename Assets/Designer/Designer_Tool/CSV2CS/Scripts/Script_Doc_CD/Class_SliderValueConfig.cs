using UnityEngine; 
using System.Collections; 
public class Class_SliderValueConfig { 
	public string LevelID { get; set; }    //关卡编号
	public int _LevelID (){
		int value = int.Parse(LevelID);
		return value;
	}
	public string SliderRequireValue { get; set; }    //"Slider的要求值
	  public string _SliderRequireValue (){
		string value = SliderRequireValue;
		return value;
	}
	public string SliderNames { get; set; }    //用分号隔开"
	  public string _SliderNames (){
		string value = SliderNames;
		return value;
	}
	}
	