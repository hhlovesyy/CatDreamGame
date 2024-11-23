using UnityEngine; 
using System.Collections.Generic; 
public static class SD_CatgameCommonItem { 
	public static Dictionary<string, Class_CatgameCommonItem> Class_Dic = JsonReader.ReadJson<Class_CatgameCommonItem> ("Json/Document/CatgameCommonItem");
	}
