using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private const string SAVE_EXTENSION = "txt";
    private static readonly string SAVE_FOLDER = "HighScore";
    //private static readonly string SAVES_FOLDER = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), SAVE_FOLDER);
    private static readonly string SAVES_FOLDER = Path.Combine(Application.dataPath, SAVE_FOLDER);
    private static bool isInit = false;

    public static void Init()
    {
        if (!isInit)
        {
            isInit = true;
            //Test if Save Folder Exist
            if (!Directory.Exists(SAVES_FOLDER))
            {
                //Create Save Folder
                Directory.CreateDirectory(SAVES_FOLDER);
            }
        }
    }

    public static void Save(string fileName, string saveString, bool overWrite)
    {
        Debug.Log(saveString);
        Init();
        string saveFileName = fileName + "." + SAVE_EXTENSION;
        if (!overWrite)
        {
            // Making sure the save number is always unique
            int saveNumber = 1;
            while (File.Exists(SAVES_FOLDER + saveFileName))
            {
                saveNumber++;
                saveFileName = fileName + "_" + saveNumber;
            }

        }

        string filePath = Path.Combine(SAVES_FOLDER, saveFileName);

        File.WriteAllText(filePath, saveString);
    }

    public static string Load(string fileName)
    {
        Init();

        string saveFileName = fileName;
        string filePath = Path.Combine(SAVES_FOLDER, saveFileName);
        if (File.Exists(filePath))
        {
            string saveString = File.ReadAllText(filePath);
            return saveString;
        }
        else
        {
            return null;
        }
    }

    public static string LoadMostRecentFile()
    {
        Init();

        DirectoryInfo directoryInfo = new DirectoryInfo(SAVES_FOLDER);
        //Get all save files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        //Cycling thru all files and indetify the most recent
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles)
        {
            Debug.Log("File found: " + fileInfo.Name + " LastWriteTime: " + fileInfo.LastWriteTime);
            if (mostRecentFile == null)
            {
                mostRecentFile = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileInfo;
                }
            }
        }

        if (mostRecentFile != null)
        {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            //Debug.Log(mostRecentFile);
            return saveString;
        }
        else
        {
            Debug.LogWarning("No save files found.");
            return null;
        }
    }

    public static void SaveObject(object saveObject)
    {
        SaveObject("save", saveObject, true);
    }

    public static void SaveObject(string fileName, object saveObject, bool overwrite)
    {
        Init();
        string json = JsonUtility.ToJson(saveObject);
        Save(fileName, json, overwrite);
    }

    public static TSaveObject LoadMostRecentObject<TSaveObject>()
    {
        Init();
        string saveString = LoadMostRecentFile();

        if (saveString != null)
        {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        }
        else
        {
            //Debug.Log("Null woi");
            return default(TSaveObject);
        }
    }

    public static TSaveObject LoadObject<TSaveObject>(string fileName)
    {
        Init();
        string saveString = Load(fileName);
        if (saveString != null)
        {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        }
        else
        {
            return default(TSaveObject);
        }
    }
}
