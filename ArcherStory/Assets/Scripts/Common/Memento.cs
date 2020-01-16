using System;
using System.Collections.Generic;
using System.Text;
using LitJson;
using System.IO;
using UnityEngine;

public class Memento
{
    public void SaveByJson()
    {
        PlayerData pd = GameRoot.instance.Playerdata;
        string filePath = Application.streamingAssetsPath + Constants.jsonPath; 
        string saveJson= JsonMapper.ToJson(pd);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJson);
        sw.Close();
    }
    public PlayerData ReadByJsonFile()
    {
        PlayerData pd = new PlayerData();
        string filePath = Application.streamingAssetsPath + Constants.jsonPath;
        StreamReader sr = new StreamReader(filePath);
        string jsonSr = sr.ReadToEnd();
        sr.Close();
        pd = JsonMapper.ToObject<PlayerData>(jsonSr);

        return pd;
    }
}
