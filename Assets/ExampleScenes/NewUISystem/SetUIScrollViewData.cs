using System.Collections;
using System.Collections.Generic;
using OurGameFramework;
using UnityEngine;

//类型的Enum
public enum GenshinDemoListType
{
    Weapon,
    Mineral, //矿石
    MissionItem, //任务物品
    Food, //食物
    Material, //材料
    Artifact, //圣遗物
    Other, //其他
}
public class GenshinDemoListData
{
    public int id;
    public string name;
    public GenshinDemoListType type;

    public float getTime; //获取时间
    public int count; //数量

    public int level;
    //...其他变量，比如获取时间等
    public int selectCount = 0; //当前某个物品选择的数量
    public bool multiSelectInOneItem = false; //是否在一个格子中显示多个数量
}

public class GenshinUserDataStruct
{
    //这个类存储相关打开背包的逻辑，比如可选择物品的个数限制，物体是否要叠加显示，是否能显示X键取消勾选等
    public int maxSelectCount = 1; //最大可选择种类数量
    public bool isShowX = true; //是否显示X键
    public bool isCanOverlap = false; //是否能叠加显示,比如100把武器，要在一个格子中显示还是在多个格子中显示
    public bool canRangeSelect = false; //是否可以范围选择
}

public class SetUIScrollViewData : MonoBehaviour
{
    public UIScrollView UIScrollView;
    public GameObject Item;
    public GameObject GenshinItem;
    void Start()
    {
        SetGenshinDataToUIScrollViewWeapon();
        //SetGenshinDataToUIScrollViewMineral();
        //SetGenshinDataToScrollViewCanRangeSelect();
    }

    private void SetGenshinDataToScrollViewCanRangeSelect()
    {
        //关于武器的逻辑，可以多选，不会叠加显示
        List<GenshinDemoListData> dataList = new List<GenshinDemoListData>();
        for (int i = 0; i < 5000; i++)
        {
            GenshinDemoListData data = new GenshinDemoListData();
            data.id = i;
            data.name = i.ToString();
            data.count = 1;
            data.level = 80;
            data.type = GenshinDemoListType.Weapon;
            dataList.Add(data);
        }
        //创建对应的UserData，方便格子的逻辑处理
        GenshinUserDataStruct userData = new GenshinUserDataStruct();
        userData.maxSelectCount = 10000; //可以最多选择10000个
        userData.isShowX = true; //显示X键，只做UI上的显示，true或false都可以再次点击取消选择
        userData.isCanOverlap = true; //不叠加显示
        userData.canRangeSelect = true;
        
        UIScrollView.SetUpList(dataList, GenshinItem, typeof(UIGenshinItem),false, userData);
    }
    
    private void SetGenshinDataToUIScrollViewWeapon()
    {
        //关于武器的逻辑，可以多选，不会叠加显示
        List<GenshinDemoListData> dataList = new List<GenshinDemoListData>();
        for (int i = 0; i < 5000; i++)
        {
            GenshinDemoListData data = new GenshinDemoListData();
            data.id = i;
            data.name = i.ToString();
            data.count = 1;
            data.level = 80;
            data.type = GenshinDemoListType.Weapon;
            dataList.Add(data);
        }
        //创建对应的UserData，方便格子的逻辑处理
        GenshinUserDataStruct userData = new GenshinUserDataStruct();
        userData.maxSelectCount = 100; //可以最多选择10000个
        userData.isShowX = true; //显示X键，只做UI上的显示，true或false都可以再次点击取消选择
        userData.isCanOverlap = true; //不叠加显示
        
        UIScrollView.SetUpList(dataList, GenshinItem, typeof(UIGenshinItem),false, userData);
    }
    
    private void SetGenshinDataToUIScrollViewMineral()
    {
        //关于武器的逻辑，可以多选，不会叠加显示
        List<GenshinDemoListData> dataList = new List<GenshinDemoListData>();
        for (int i = 0; i < 500; i++)
        {
            GenshinDemoListData data = new GenshinDemoListData();
            data.id = i;
            data.name = i.ToString();
            data.count = 200;
            data.level = 80;
            data.type = GenshinDemoListType.Mineral;
            data.multiSelectInOneItem = true;
            dataList.Add(data);
        }
        //创建对应的UserData，方便格子的逻辑处理
        GenshinUserDataStruct userData = new GenshinUserDataStruct();
        userData.maxSelectCount = 100; //可以最多选择10000个
        userData.isShowX = true; //显示X键，只做UI上的显示，true或false都可以再次点击取消选择
        userData.isCanOverlap = false; //不叠加显示
        
        UIScrollView.SetUpList(dataList, GenshinItem, typeof(UIGenshinItem),false, userData);
    }
    
    private void SetDataToUIScrollView()
    {
        List<int> dataList = new List<int>();
        for (int i = 0; i < 5000; i++)
        {
            dataList.Add(i);
        }
        UIScrollView.SetUpList(dataList, Item, typeof(UITestItem));
        //UIScrollView.Select(10);
    }
}
