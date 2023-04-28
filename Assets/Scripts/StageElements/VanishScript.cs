using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishScript : MonoBehaviour
{
    public Animator Anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Anim.SetBool("Trigger", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Anim.SetBool("Trigger", false);
    }
}
