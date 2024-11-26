using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Assertions;

public class CatGamePlayerData
{
    public List<int> levelBestTimes = new List<int>();
    public List<int> levelBestStars = new List<int>();
}

public class CatGameXMLReader: Singleton<CatGameXMLReader>
{
    public void SavePlayerData(int levelIndex, int useTime, int starLevel)
    {
        string saveXmlPath = Application.dataPath + "/Designer/XMLTable/CatGamePlayerStorage.xml";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(saveXmlPath);
        XmlElement root = xmlDoc.DocumentElement;
        
        XmlNode levelBaseInfoNode = root.SelectSingleNode("LevelBaseInfo");
        if (levelBaseInfoNode != null)
        {
            //<LevelBaseInfoNode playerCurrentUpperLevel="1" />
            XmlNode levelBaseInfo = levelBaseInfoNode.SelectSingleNode("LevelBaseInfoNode");
            if (levelBaseInfo != null) 
            {
                levelBaseInfo.Attributes["playerCurrentUpperLevel"].Value = (levelIndex + 1).ToString();
                xmlDoc.Save(saveXmlPath);
            }
        }
        
        XmlNode levelDataInfoNode = root.SelectSingleNode("LevelDataInfo");
        
        if (levelDataInfoNode != null)
        {
            XmlNodeList levelsNode = levelDataInfoNode.SelectNodes("LevelDataInfoNode");
            if (levelsNode.Count != 0)
            {
                for (int i = 0; i < levelsNode.Count; i++)
                {
                    if (int.Parse(levelsNode[i].Attributes["levelIndex"].Value) == levelIndex)
                    {
                        levelsNode[i].Attributes["bestTime"].Value = useTime.ToString();
                        levelsNode[i].Attributes["starLevel"].Value = starLevel.ToString();
                        xmlDoc.Save(saveXmlPath);
                        return;
                    }
                }
            }
        }
    }

    public void ResetPlayerData()  //这个函数主要是自己用来测试的，不会在发布游戏中使用
    {
        string saveXmlPath = Application.dataPath + "/Designer/XMLTable/CatGamePlayerStorage.xml";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(saveXmlPath);
        XmlElement root = xmlDoc.DocumentElement;
        
        XmlNode levelBaseInfoNode = root.SelectSingleNode("LevelBaseInfo");
        if (levelBaseInfoNode != null)
        {
            //<LevelBaseInfoNode playerCurrentUpperLevel="1" />
            XmlNode levelBaseInfo = levelBaseInfoNode.SelectSingleNode("LevelBaseInfoNode");
            if (levelBaseInfo != null) 
            {
                levelBaseInfo.Attributes["playerCurrentUpperLevel"].Value = "1";
                xmlDoc.Save(saveXmlPath);
            }
        }
        
        XmlNode levelDataInfoNode = root.SelectSingleNode("LevelDataInfo");
        
        if (levelDataInfoNode != null)
        {
            XmlNodeList levelsNode = levelDataInfoNode.SelectNodes("LevelDataInfoNode");
            if (levelsNode.Count != 0)
            {
                for (int i = 0; i < levelsNode.Count; i++)
                {
                    levelsNode[i].Attributes["bestTime"].Value = "-1";
                    levelsNode[i].Attributes["starLevel"].Value = "-1";
                    xmlDoc.Save(saveXmlPath);
                }
            }
        }
    }
    
    public CatGamePlayerData ReadPlayerData()
    {
        CatGamePlayerData playerData = new CatGamePlayerData();
        string saveXmlPath = Application.dataPath + "/Designer/XMLTable/CatGamePlayerStorage.xml";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(saveXmlPath);
        XmlElement root = xmlDoc.DocumentElement;
        XmlNode levelBaseInfoNode = root.SelectSingleNode("LevelBaseInfo");
        if (levelBaseInfoNode != null)
        {
            //<LevelBaseInfoNode playerCurrentUpperLevel="1" />
            XmlNode levelBaseInfo = levelBaseInfoNode.SelectSingleNode("LevelBaseInfoNode");
            if (levelBaseInfo != null) 
            {
                HGameRoot.Instance.currentMaxLevel = int.Parse(levelBaseInfo.Attributes["playerCurrentUpperLevel"].Value);
            }
        }
            
        XmlNode levelDataInfoNode = root.SelectSingleNode("LevelDataInfo");
        if (levelDataInfoNode != null)
        {
            XmlNodeList levelsNode = levelDataInfoNode.SelectNodes("LevelDataInfoNode");
            if (levelsNode.Count != 0)
            {
                for (int i = 0; i < levelsNode.Count; i++)
                {
                    int levelID = int.Parse(levelsNode[i].Attributes["levelIndex"].Value);
                    Assert.AreEqual(levelID, i + 1);
                    playerData.levelBestTimes.Add(int.Parse(levelsNode[i].Attributes["bestTime"].Value));
                    playerData.levelBestStars.Add(int.Parse(levelsNode[i].Attributes["starLevel"].Value));
                }
            }
        }

        return playerData;
    }
}
