using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapControl : MonoBehaviour
{
    public GameObject Player;
    public float Timer, GoalTime;
    public bool Moving;
    public MapNode SelNode, TarNode;
    public List<MapNode> Nodes;
    public List<bool> LvCompletion;

    // Start is called before the first frame update
    void Start()
    {
        Moving = false;
        SelNode = Nodes[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving == false)
        {
            Player.transform.position = SelNode.transform.position;

            if (Input.GetAxisRaw("Horizontal") < 0 && SelNode.LeftN != null)
            {
                TarNode = SelNode.LeftN;
                Moving = true;
            }

            if (Input.GetAxisRaw("Horizontal") > 0 && SelNode.RightN != null)
            {
                TarNode = SelNode.RightN;
                Moving = true;
            }

            if (Input.GetAxisRaw("Vertical") > 0 && SelNode.TopN != null)
            {
                TarNode = SelNode.TopN;
                Moving = true;
            }

            if (Input.GetAxisRaw("Vertical") < 0 && SelNode.BottomN != null)
            {
                TarNode = SelNode.BottomN;
                Moving = true;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                GoToLevel(SelNode.LvName);
            }
        }
        else
        {
            Timer += Time.deltaTime / GoalTime;
            Player.transform.position = Vector3.Lerp(SelNode.transform.position, TarNode.transform.position, Timer);
            if(Timer >= 1)
            {
                SelNode = TarNode;
                Moving = false;
                Timer = 0;
            }
        }
    }

    public void GoToLevel(string LvID)
    {
        SceneManager.LoadScene(LvID);
    }

    public void ClearLevel(string LvID)
    {
        PlayerPrefs.SetInt("Lv" + LvID + "Completed", 1);
    }
}
