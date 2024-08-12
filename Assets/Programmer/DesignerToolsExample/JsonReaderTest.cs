using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonReaderTest : MonoBehaviour
{
    private string questionPath;
    private List<string> contents = new List<string>();
    private List<string> labels = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        questionPath = "Assets/Designer/JsonFile/usual_test_labeled.json";
        LoadQuestionFromDataset();
    }
    
    private void LoadQuestionFromDataset()
    {
        string jsonString = File.ReadAllText(questionPath, Encoding.UTF8);
        //seperate each object
        JArray jArray = JArray.Parse(jsonString);
        foreach (JObject obj in jArray.Children<JObject>())
        {
            foreach (JProperty singleProp in obj.Properties())
            {
                if (singleProp.Name == "content")
                {
                    contents.Add(singleProp.Value.ToString());
                }
                else if (singleProp.Name == "label")
                {
                    labels.Add(singleProp.Value.ToString());
                }
                    
            }
        }
    }
}
