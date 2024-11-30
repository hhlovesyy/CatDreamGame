using UnityEngine; 
using System.Collections.Generic; 
public static class SD_LevelMasterSay { 
	public static Dictionary<string, Class_LevelMasterSay> Class_Dic = JsonReader.ReadJson<Class_LevelMasterSay> ("Json/Document/LevelMasterSay");
	}
