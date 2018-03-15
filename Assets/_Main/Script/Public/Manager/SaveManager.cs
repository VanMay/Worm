using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    public static SaveManager instance;
    public bool autoSave = true;
    private ES3Settings saveSetting;
    [SerializeField]
    private int currentSaveSlot;
    private string[] filePath;
	
	void Awake () {
        instance = this;

        saveSetting = new ES3Settings(ES3.EncryptionType.AES, "Password");
        saveSetting.location = ES3.Location.File;
        filePath = new string[]
        {
            Application.dataPath + "/_SaveFile/Slot1/",
            Application.dataPath + "/_SaveFile/Slot2/",
            Application.dataPath + "/_SaveFile/Slot3/"
        };
    }

    /// <summary>
    /// 存储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="fileName"></param>
    public void Save<T>(string key, object value, string fileName)
    {
        ES3.Save<T>(key, value, filePath[currentSaveSlot] + fileName, saveSetting);
    }

    /// <summary>
    /// 读取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T Load<T>(string key, string fileName)
    {
        return ES3.Load<T>(key, filePath[currentSaveSlot] + fileName, saveSetting);
    }

    /// <summary>
    /// 检测是否有存档
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool FileExists(string fileName)
    {
        return ES3.FileExists(filePath[currentSaveSlot] + fileName, saveSetting);
    }
}
