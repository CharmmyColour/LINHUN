using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTesting : MonoBehaviour
{
    //External elements
    public Rigidbody2D RB2D;
    public Transform GroundPoint, CealingPoint, WallPointA, WallPointB;
    public bool Grounded, Cealed, Walled;
    public LayerMask IsGround;

    //States
    public enum CharStates { Normal, FloUp, FloDown, WallHang };
    public CharStates CurrState;
    public Color NoJColor, OneJColor;
    public bool Tangible;
    public float TangiTimer;

    //Stats
    public float Speed, JumpSpeed;
    public int AirJumps, MaxJumps;

    //Keeping track
    private int MoveX, MoveY;
    public Vector2 RespawnPos;

    //Animations
    public Animator anim;
    private bool isGrounded, isWalled, Jumping, Falling;

    // Start is called before the first frame update
    void Start()
    {
        Tangible = true;
        TangiTimer = 0;
        RespawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveX = (int)Input.GetAxisRaw("Horizontal");
        MoveY = (int)Input.GetAxisRaw("Vertical");
        Grounded = Physics2D.OverlapCircle(GroundPoint.position, 0.05f, IsGround);
        Cealed = Physics2D.OverlapCircle(CealingPoint.position, 0.05f, IsGround);
        Walled = Physics2D.OverlapArea(WallPointA.position, WallPointB.position, IsGround);

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWalled", isWalled);
        anim.SetBool("isJumping", Jumping);
        anim.SetBool("isFalling", Falling);

        anim.SetInteger("SpeedMovement", Mathf.RoundToInt(MoveX));

        if (Grounded == true)
        {
            AirJumps = 0;
            isGrounded = true;
        }

        if (Tangible == true)
        {
            Physics2D.IgnoreLayerCollision(3, 7, false);
            TangiTimer = 0;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Tangible = false;
            }
        }
        else
        {
            Physics2D.IgnoreLayerCollision(3, 7, true);
            TangiTimer += Time.deltaTime;
            if (TangiTimer >= 1)
            {
                Tangible = true;
            }
        }

        switch (CurrState)
        {
            case CharStates.Normal:
                RB2D.gravityScale = 1;
                isGrounded = true;
                isWalled = false;
                Jumping = false;
                Falling = false;

                if (MoveX != 0 && RB2D.velocity.y >= 0)
                {
                       transform.localScale = new Vector3(1 * MoveX, 1, 1);
                        transform.position += new Vector3(transform.localScale.x, 0, 0) * Speed * Time.deltaTime;
                }

                if (Input.GetButtonDown("Jump") && (Grounded == true || AirJumps > 0))
                {
                    CurrState = CharStates.FloUp;
                    AirJumps -= 1;
                }
                break;
            case CharStates.FloUp:
                RB2D.velocity = new Vector3(Speed * transform.localScale.x, JumpSpeed);
                isGrounded = false;
                isWalled = false;
                Jumping = true;
                Falling = false;

                if (Input.GetButtonDown("Jump"))
                {
                    CurrState = CharStates.FloDown;
                }
                break;
            case CharStates.FloDown:
                RB2D.velocity = new Vector3(Speed * transform.localScale.x, -JumpSpeed);
                isGrounded = false;
                isWalled = false;
                Jumping = false;
                Falling = true;

                if (Input.GetButtonDown("Jump") && AirJumps > 0)
                {
                    CurrState = CharStates.FloUp;
                    AirJumps -= 1;
                }
                break;
            case CharStates.WallHang:
                RB2D.velocity = new Vector2(0, 0);
                RB2D.gravityScale = 0;
                isGrounded = false;
                isWalled = true;
                Jumping = false;
                Falling = false;

                if (Input.GetButtonDown("Jump"))
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                    CurrState = CharStates.FloUp;
                    AirJumps -= 1;
                }
                break;
        }

        if (transform.position.y < -20) { Death(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            CurrState = CharStates.Normal;
            RB2D.velocity = new Vector2(0, 0);
        }
        if (collision.transform.tag == "Grabbable")
        {
            if (Walled == true)
            {
                CurrState = CharStates.WallHang;
                RB2D.velocity = new Vector2(0, 0);
            }
        }
        if (collision.transform.tag == "Danger")
        {
            Death();
        }
    }

    public void JumpsRestore()
    {
        AirJumps = MaxJumps;
    }

    public void Death()
    {
        transform.position = RespawnPos;
        Jumping = false;
        Falling = false;
    }
}
