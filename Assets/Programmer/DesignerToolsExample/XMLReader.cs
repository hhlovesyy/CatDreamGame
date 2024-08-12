using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XMLReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string gachaSaveXmlPath = Application.dataPath + "/Designer/XMLTable/gachaHistory.xml";
        //加载抽卡的xml文件
        XmlDocument xmlDoc = new XmlDocument();
        //加载抽卡记录
        xmlDoc.Load(gachaSaveXmlPath);
        List<string> names = new List<string>();
        List<string> stars = new List<string>();
        List<string> timeStrs = new List<string>();
        // 获取根节点
        XmlElement root = xmlDoc.DocumentElement;
        XmlNode historyNode = root.SelectSingleNode("GachaHistoryInfo");
        if (historyNode != null)
        {
            XmlNodeList levelsNode = historyNode.SelectNodes("GachaResultNode");
            //Sort by time
            if (levelsNode.Count != 0)
            {
                //取currentPage页的数据，一页显示10个
                for (int i = 0; i < levelsNode.Count; i++)
                {
                    names.Add(levelsNode[i].Attributes["name"].Value);
                    stars.Add(levelsNode[i].Attributes["star"].Value);
                    timeStrs.Add(levelsNode[i].Attributes["timeStr"].Value);
                }
            }
        }

        SaveBaseDataToXmlFile(xmlDoc, gachaSaveXmlPath);
    }
    
    private void SaveBaseDataToXmlFile(XmlDocument xmlDoc, string savePath)
    {
        //对这些数值进行更新
        int test1 = 100;
        int test2 = 200;
        bool test3 = true;
        bool test4 = true;
        //把已经抽了的抽数保存到Xml文件当中
        XmlElement root = xmlDoc.DocumentElement;
        // 创建新的character节点
        XmlNode historyNode = root.SelectSingleNode("GachaBaseInfo");
        if (historyNode != null)
        {
            XmlNodeList levelsNode = historyNode.SelectNodes("GachaBaseInfoNode");
            if (levelsNode.Count == 0) //还没有抽卡记录，就创建一个新的节点
            {
                XmlElement newGachaBaseNode = xmlDoc.CreateElement("GachaBaseInfoNode");
                newGachaBaseNode.SetAttribute("drawCounter5Star", test1.ToString());
                newGachaBaseNode.SetAttribute("drawCounter4Star", test2.ToString());
                newGachaBaseNode.SetAttribute("isLast5StarCharacterUp", test3.ToString());
                newGachaBaseNode.SetAttribute("isLast4StarUp", test4.ToString());

                // 将新创建的节点添加到levelsNode节点中
                if (historyNode!=null)
                {
                    historyNode.AppendChild(newGachaBaseNode);
                }
            }
            else
            {
                foreach (XmlElement xe in levelsNode) 
                {
                    xe.SetAttribute("drawCounter5Star", test1.ToString());
                    xe.SetAttribute("drawCounter4Star", test2.ToString());
                    xe.SetAttribute("isLast5StarCharacterUp", test3.ToString());
                    xe.SetAttribute("isLast4StarUp", test4.ToString());
                }
            }
        }
        xmlDoc.Save(savePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
