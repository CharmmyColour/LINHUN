using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    
    public static int CurrSave, CurrLevel;
    //Save data
    [System.Serializable]
    public class LvData
    {
        public string LvName;
        public bool LvComplete;
        public List<bool> Collectibles;
    }
    public List<LvData> LvsList;

    public void CreateSave(int SaveID)
    {
        CurrSave = SaveID;
        for (int i = 0; i < LvsList.Count; i++)
        {
            PlayerPrefs.SetInt("Save" + SaveID.ToString() + "Lv" + i.ToString() + "Conplete", 0);
        }
    }

    public void LoadSave(int SaveID)
    {
        for (int i = 0; i < LvsList.Count; i++)
        {
            LvsList[i].LvComplete = PlayerPrefs.GetInt("Save" + SaveID.ToString() + "Lv" + i.ToString() + "Conplete", 0) != 0;
        }
    }

    public void SaveGame(int SaveID)
    {
        for (int i = 0; i < LvsList.Count; i++)
        {
            PlayerPrefs.SetInt("Save" + SaveID.ToString() + "Lv" + i.ToString() + "Conplete", LvsList[i].LvComplete ? 1:0);
        }
    }

    public static void LevelClear()
    {
        PlayerPrefs.SetInt("Save" + CurrSave.ToString() + "Lv" + CurrLevel.ToString() + "Conplete", 1);
    }
}
