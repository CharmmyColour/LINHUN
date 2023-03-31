using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public List<Transform> TargetPos;
    public int CurTarPos;
    public bool Moving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Moving == true && transform.position != TargetPos[CurTarPos].position)
        {

        }
    }
}
