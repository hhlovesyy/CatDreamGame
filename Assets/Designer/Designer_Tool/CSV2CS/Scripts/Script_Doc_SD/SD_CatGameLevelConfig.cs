using UnityEngine; 
using System.Collections.Generic; 
public static class SD_CatGameLevelConfig { 
	public static Dictionary<string, Class_CatGameLevelConfig> Class_Dic = JsonReader.ReadJson<Class_CatGameLevelConfig> ("Json/Document/CatGameLevelConfig");
	}
