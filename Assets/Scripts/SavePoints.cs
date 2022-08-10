using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SerializableListV2<Vector2>
{
    public List<Vector2> list2;
}

[Serializable]
public static class SavePoints
{
    public static string directory = "/SaveData/";
    public static string fileName = "PolygonPionts.txt";
    [SerializeField]
    //public static SerializableListV2<Vector2> listv2;
    //public static SerializableListV2<Vector2> listv2;

    public static void Save(SerializableList<Vector2> list)
    {
        //SerializableListV2<Vector2> listv2 = new SerializableListV2<Vector2
        /*foreach (Vector3 v in list)
        {
           listv2.list2.Add(new Vector2(v.x, v.z));
        }*/

        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(list);
        File.WriteAllText(dir + fileName, json);
    }
    /*
    public static void Save(Vector3 list)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(list);
        File.WriteAllText(dir + fileName, json);
    }*/

    public static void Save(Vector3[] list)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(list);
        File.WriteAllText(dir + fileName, json);
    }

    public static List<Vector3> Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        List<Vector3> list = new List<Vector3>();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            list = JsonUtility.FromJson<List<Vector3>>(json);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }

        return list;
    }
}
