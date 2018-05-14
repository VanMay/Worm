using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
    [Header("Player Info")]
    public int Ammo;
    public int WeaponListIndex;
    public List<int> WeaponIDList;

    private string fileName = "Player";

    void Start()
    {
        //Load();
    }

    /// <summary>
    /// 存储
    /// </summary>
    public void Save()
    {
        SaveManager.instance.Save<int>("Ammo", Ammo, fileName);
        SaveManager.instance.Save<int>("WeaponListIndex", WeaponListIndex, fileName);
        SaveManager.instance.Save<List<int>>("WeaponIDList", WeaponIDList, fileName);
    }
    /// <summary>
    /// 导入
    /// </summary>
    public void Load()
    {
        if (SaveManager.instance.FileExists(fileName))
        {
            Ammo = SaveManager.instance.Load<int>("Ammo", fileName);
            WeaponListIndex = SaveManager.instance.Load<int>("WeaponListIndex", fileName);
            WeaponIDList = SaveManager.instance.Load<List<int>>("WeaponIDList", fileName);
        }
    }
}
