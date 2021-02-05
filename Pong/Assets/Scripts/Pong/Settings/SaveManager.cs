using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance = null;
    public static List<ISaveLoad> m_managed = new List<ISaveLoad>();

    private void Awake()
    {
        Instance = this;
    }

    public void SaveData()
    {
        foreach (var save in m_managed)
        {
            save.Save();
        }
    }
    public void LoadData()
    {
        foreach (var load in m_managed)
        {
            load.Load();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();   
    }
}
