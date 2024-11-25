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
    public CatGamePlayerData ReadPlayerData()
    {
        CatGamePlayerData playerData = new CatGamePlayerData();
        string saveXmlPath = Application.dataPath + "/Designer/XMLTable/CatGamePlayerStorage.xml";
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(saveXmlPath);
        XmlElement root = xmlDoc.DocumentElement;
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
