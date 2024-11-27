using UnityEngine; 
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
	  public float _ItemWeight (){
		float value = float.Parse(ItemWeight);
		return value;
	}
	public string ItemCollisionShow { get; set; }    //碰到的表现
	  public string _ItemCollisionShow (){
		string value = ItemCollisionShow;
		return value;
	}
	public string ItemHitShow { get; set; }    //拍到的表现
	  public string _ItemHitShow (){
		string value = ItemHitShow;
		return value;
	}
	public string ItemHitGroundShow { get; set; }    //撞击地面的表现
	  public string _ItemHitGroundShow (){
		string value = ItemHitGroundShow;
		return value;
	}
	public string EffFloorResourceShort { get; set; }    //是否留下地面特殊特效短（null就是无否则写入addlink ）
	  public string _EffFloorResourceShort (){
		string value = EffFloorResourceShort;
		return value;
	}
	public string EffFloorResourceLong { get; set; }    //是否留下地面特殊特效长时间
	  public string _EffFloorResourceLong (){
		string value = EffFloorResourceLong;
		return value;
	}
	public string CollisionImpactSlider { get; set; }    //碰到是否会影响slider
	public int _CollisionImpactSlider (){
		int value = int.Parse(CollisionImpactSlider);
		return value;
	}
	public string hasSpecialInteraction { get; set; }    //是否有特殊交互判断
	public int _hasSpecialInteraction (){
		int value = int.Parse(hasSpecialInteraction);
		return value;
	}
	public string BrokenVfxLink { get; set; }    //BROKEN特效AddLink（跟之前的地面不同在于子任何地方broken都会有这个特效）
	  public string _BrokenVfxLink (){
		string value = BrokenVfxLink;
		return value;
	}
	}
	