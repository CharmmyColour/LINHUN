using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchChangers : MonoBehaviour
{
    public Collider2D MyColl;
    public Animator Anim;

    // Start is called before the first frame update
    void Start()
    {
        Anim.SetBool("State", MyColl.enabled);
    }

    public void Activation()
    {
        if(MyColl.enabled == true)
        {
            MyColl.enabled = false;
        }
        else { MyColl.enabled = true; }
        Anim.SetBool("State", MyColl.enabled);
    }
}
