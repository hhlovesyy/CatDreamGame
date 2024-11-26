using System.Collections;
using System.Collections.Generic;
using OurGameFramework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class HLevelManager: SingletonMono<HLevelManager>
{
    Dictionary<int, GameObject> levelDic = new Dictionary<int, GameObject>();
    private int curLevel = -1;
    public Transform levelParent;
    private void OnLevelLoaded(GameObject obj)
    {
        Debug.Log("OnLevelLoaded, good!!");
        //准备instantiate一个场景出来
        GameObject level = Instantiate(obj);
        Class_CatGameLevelConfig levelConfig = SD_CatGameLevelConfig.Class_Dic[curLevel.ToString()];
        string position = levelConfig.levelPosition;
        float x = float.Parse(position.Split(';')[0]);
        float y = float.Parse(position.Split(';')[1]);
        float z = float.Parse(position.Split(';')[2]);
        levelParent = level.transform;
        level.transform.position = new Vector3(x,y,z);
        
        levelDic.Add(curLevel, level);
    }

    public AsyncOperationHandle EnterNextLevel()
    {
        curLevel += 1;
        return LoadOneLevel(curLevel);
    }

    public AsyncOperationHandle RestartEnterThisLevel()
    {
        return LoadOneLevel(curLevel);
    }
    
    public void ClearAllLevels()
    {
        ClearHistoryLevel();
    }

    private void ClearHistoryLevel()
    {
        foreach (var level in levelDic)
        {
            Destroy(level.Value);
        }
        levelDic.Clear();
    }
    
    public AsyncOperationHandle LoadOneLevel(int levelID)
    {
        ClearHistoryLevel();
        curLevel = levelID;
        string addressableLink = "Level" + levelID + "Scene";
        //addressableLink
        return ResourceManager.Instance.LoadAssetAsync<GameObject>(addressableLink, OnLevelLoaded);
    }
}
