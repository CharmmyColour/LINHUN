using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float Timer, GoalTime;
    public Transform Pos1, Pos2;
    public bool Going1, Moving;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Counting time
        Timer += Time.deltaTime;

        //Moving
        if (Moving == true)
        {
            if (Going1 == true)
            {
                transform.position = Vector3.Lerp(Pos2.position, Pos1.position, Timer);
            }
            else
            {
                transform.position = Vector3.Lerp(Pos1.position, Pos2.position, Timer);
            }

            if (Timer >= GoalTime) { Moving = false; Timer = 0; }
        }
        //Once in position, waiting to move again
        else
        {
            if (Timer >= GoalTime) { Moving = true; Going1 = !Going1; Timer = 0; }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }
}
