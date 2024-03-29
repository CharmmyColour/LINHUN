using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTesting : MonoBehaviour 
{
    public PlayerControl MyPlayer;
    //Animations
    public Animator anim;
    public bool isGrounded, isHanging, isJumping, isFalling, isHurt, isDead;
    public int Airjumps;

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isHanging", isHanging);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);
        anim.SetInteger("jumps", MyPlayer.AirJumps);
        anim.SetInteger("SpeedMovement", Mathf.RoundToInt(MyPlayer.MoveX));

        
        if (PlayerControl.Grounded == true)
        {
            isGrounded = true;
        }

        if (PlayerControl.CurrState == PlayerControl.CharStates.Normal)
        {
            isGrounded = true;
            isHanging = false;
            isJumping = false;
            isFalling = false;
        }
        else if (PlayerControl.CurrState == PlayerControl.CharStates.FloUp)
        {
            isGrounded = false;
            isHanging = false;
            isJumping = true;
            isFalling = false;
        }
        else if (PlayerControl.CurrState == PlayerControl.CharStates.FloDown)
        {
            isGrounded = false;
            isHanging = false;
            isJumping = false;
            isFalling = true;
        }
        else if (PlayerControl.CurrState == PlayerControl.CharStates.WallHang)
        {
            isGrounded = false;
            isHanging = true;
            isJumping = false;
            isFalling = false;
        }
        Airjumps = MyPlayer.AirJumps;
    }
}
