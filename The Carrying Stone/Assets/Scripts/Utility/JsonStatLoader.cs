using UnityEngine;
using System.IO;

public static class JsonStatLoader
{
    private static string tempPath;
    private static string path = "/Resources/JsonFiles";
    private static string jsonString;


    //Converts the data from the JSON file to an Object called Data
    public static void DataBuilder(string _fileName)
    {
        tempPath = Application.streamingAssetsPath + path + "/" + _fileName + ".json";
        if (File.Exists(tempPath))
        {
            jsonString = File.ReadAllText(tempPath);
            //Debug.Log("Data from Jsonfile:" + jsonString);
        }
        else
        {
            Debug.Log("Cannot Load File - File name does not exist");
        }
    }
    
    //Loads data based on teh path name and the type of data object to be loaded
    //Overload this method if any other type of data should be loaded
    public static BaseAttributes DataLoader(string _fileName)
    {
        DataBuilder(_fileName);
        BaseAttributes Data = JsonUtility.FromJson<BaseAttributes>(jsonString);
        return Data;
    }
}
