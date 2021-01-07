using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance = null;
    public List<ISave> m_saveDatas = new List<ISave>();

    private void Awake()
    {
        Instance = this;
    }

    public void SaveData()
    {
        foreach (var save in m_saveDatas)
        {
            save.Save();
        }
    }
}
