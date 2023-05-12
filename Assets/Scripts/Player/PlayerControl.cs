using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    //External elements
    public Rigidbody2D RB2D;
    public Transform GroundPoint, CealingPoint, WallPointA, WallPointB;
    public static bool Grounded, Cealed, Walled, Grabbed;
    public LayerMask IsGround, IsGrab;

    //States
    public enum CharStates { Normal, FloUp, FloDown, WallHang, Death };
    public static CharStates CurrState;
    public Color NoJColor, OneJColor;
    public bool Tangible;
    public float TangiTimer;

    //Stats
    public float Speed, JumpSpeed;
    public int AirJumps, MaxJumps;

    //Keeping track
    public int MoveX, MoveY;
    public Vector3 RespawnPos;

    //Pause
    public bool Paused;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Tangible = true;
        TangiTimer = 0;
        RespawnPos = transform.position;

        Paused = false;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Paused == false)
        {
            MoveX = (int)Input.GetAxisRaw("Horizontal");
            MoveY = (int)Input.GetAxisRaw("Vertical");
            Grounded = Physics2D.OverlapCircle(GroundPoint.position, 0.05f, IsGround);
            Cealed = Physics2D.OverlapCircle(CealingPoint.position, 0.05f, IsGround);
            Walled = Physics2D.OverlapArea(WallPointA.position, WallPointB.position, IsGround);
            Grabbed = Physics2D.OverlapArea(WallPointA.position, WallPointB.position, IsGrab);

            if (Grounded == true)
            {
                AirJumps = 0;
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
                    //Action
                    RB2D.velocity = new Vector3(Speed * transform.localScale.x, JumpSpeed);

                    //Input for jump
                    if (Input.GetButtonDown("Jump"))
                    {
                        CurrState = CharStates.FloDown;
                    }

                    //Check for walls
                    if (Grabbed == true)
                    {
                        CurrState = CharStates.WallHang;
                        RB2D.velocity = new Vector2(0, 0);
                    }
                    else if (Walled == true && Tangible == true)
                    {
                        CurrState = CharStates.Normal;
                        RB2D.velocity = new Vector2(0, 0);
                    }
                    break;
                case CharStates.FloDown:
                    RB2D.velocity = new Vector3(Speed * transform.localScale.x, -JumpSpeed);

                    if (Input.GetButtonDown("Jump") && AirJumps > 0)
                    {
                        CurrState = CharStates.FloUp;
                        AirJumps -= 1;
                    }
                    if (Grounded == true && Tangible == true)
                    {
                        CurrState = CharStates.Normal;
                        RB2D.velocity = new Vector2(0, 0);
                    }
                    break;
                case CharStates.WallHang:
                    RB2D.velocity = new Vector2(0, 0);
                    RB2D.gravityScale = 0;

                    if (Input.GetButtonDown("Jump"))
                    {
                        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                        CurrState = CharStates.FloUp;
                    }
                    if (Grabbed != true)
                    {
                        CurrState = CharStates.Normal;
                        RB2D.velocity = new Vector2(0, 0);
                    }
                    if (Grounded == true && Tangible == true)
                    {
                        CurrState = CharStates.Normal;
                        RB2D.velocity = new Vector2(0, 0);
                    }
                    break;
                case CharStates.Death:
                    RB2D.velocity = new Vector2(0, 0);
                    RB2D.gravityScale = 0;
                    break;
            }

            //Death if fall very far down
            if (transform.position.y < -20) { Death(); }

            //Pause
            if (Input.GetButtonDown("Pause"))
            {
                PausePlay();
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                Unpause();
            }
        }
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
            if (Grabbed == true)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Danger")
        {
            Death();
        }

        if (collision.transform.tag == "Goal")
        {
            Victory(collision.GetComponent<ToNextLevel>().Destiny);
        }
    }

    public void JumpsRestore()
    {
        AirJumps = MaxJumps;
    }

    public void NewRespawnPoint(Vector2 NewPoint)
    {
        RespawnPos = NewPoint;
    }

    //Pause and Un-Pause
    public void PausePlay()
    {
        Paused = true;
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void Unpause()
    {
        Paused = false;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    public void Death()
    {
        Unpause();
        transform.position = RespawnPos;
        CurrState = CharStates.Normal;
        RB2D.velocity = new Vector2(0, 0);
    }

    public void ExitToMap(string Destiny)
    {
        Unpause();
        SceneManager.LoadScene(Destiny);
    }

    public void Victory(string Destiny)
    {
        SaveData.LevelClear();
        SceneManager.LoadScene(Destiny);
    }
}
