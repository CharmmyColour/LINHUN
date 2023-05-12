using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public bool Unlocked = false;
    public MapNode LeftN, RightN, TopN, BottomN;
    //The ID of the level this node goes to
    public int MyLevel;
    public string LvName;
    //The IDs of the levels that have to be completed for this one to be unlocked
    public List<int> UnlockConditions;

    // Start is called before the first frame update
    void Start()
    {
        //if(UnlockConditions.Count == 0) { Unlocked = true; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void CheckLock()
    //{
    //    int Conds = 0;
    //    for (int i = 0; i < UnlockConditions.Count; i++)
    //    {
    //        if(PlayerPrefs.GetInt("Save" + SaveData.CurrSave.ToString() + "Lv" + MyLevel.ToString() + "Conplete") == 1)
    //        {
    //            Conds++;
    //        }
    //    }
    //    if(Conds >= UnlockConditions.Count)
    //    {
    //        Unlocked = true;
    //    }
    //}
}
