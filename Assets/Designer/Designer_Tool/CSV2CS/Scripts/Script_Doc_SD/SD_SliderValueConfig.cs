using UnityEngine; 
using System.Collections.Generic; 
public static class SD_SliderValueConfig { 
	public static Dictionary<string, Class_SliderValueConfig> Class_Dic = JsonReader.ReadJson<Class_SliderValueConfig> ("Json/Document/SliderValueConfig");
	}
