using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KnightScript : MonoBehaviour {

    [SerializeField] float speed = 3f, jumpForce = 350f;
    Rigidbody2D rb;
    Animator anim;
    bool lookRight = true;
    public bool OnAttack = false;

    //grounded
    [SerializeField] bool grounded;
    [SerializeField] float groundRadius = 0.02f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask theGround;

    [SerializeField] AudioClip sndAttack, sndJump, sndHurt, sndDead, sndWin, sndGoblin,sndSkeleton, sndPickUp, sndMummy;
    AudioSource audioS;

    [SerializeField] ProgressBar pb;
    [SerializeField] GameObject canvasGO;

    //dash
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;
  //  private float DashCountDown = 2f;

    void Awake () {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
	}	
	
	void Update () {

        
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, theGround);
        anim.SetBool("Grounded", grounded);

        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * move * speed * Time.deltaTime);
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetFloat("Vspeed", rb.velocity.y);

        if(move>0 && !lookRight)
        {
            Flip();
        }
        else if(move < 0 && lookRight)
        {
            Flip();
        }


        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            audioS.PlayOneShot(sndJump);
        } 

        if (Input.GetKeyDown(KeyCode.Space) && grounded && !OnAttack)
        {
            OnAttack = true;
            anim.SetTrigger("Attack");
            audioS.PlayOneShot(sndAttack);
        }
        //dash
      //  DashCountDown -= Time.deltaTime;

        if (direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.C) && grounded/* && DashCountDown <= 0*/)
            {
                direction = 1;
            }
            
            
        }
        else
        {
            if(dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if(direction == 1)
                {
                    if (lookRight)
                    {
                        rb.velocity = Vector2.right * dashSpeed;
                      //  DashCountDown = 2f;
                    }
                    else if (!lookRight)
                    {
                        rb.velocity = Vector2.left * dashSpeed;
                      //  DashCountDown = 2f;
                    }
                    
                }
            }
        }



        ///Debug

        if (Input.GetKeyDown(KeyCode.Escape)) BackToMenu();
        if (Input.GetKeyDown(KeyCode.DownArrow)) GoDown();
        if (Input.GetKeyDown(KeyCode.A)) speed = speed * 2;

    }

    private void FixedUpdate()
    {
       /* if(Input.GetKeyDown(KeyCode.LeftControl) && grounded)
        {
            rb.AddForce(new Vector2(0, jumpForce));
          //  grounded = false;
            audioS.PlayOneShot(sndJump);
        }*/
    }


    void Flip()
    {
        lookRight = !lookRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //Go Down the plateforms
    public void GoDown()
    {
        GameObject.Find("TilemapPlateform").GetComponent<PlatformEffector2D>().surfaceArc = 0;
        StartCoroutine(timeToGoGown());
    }
    IEnumerator timeToGoGown()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("TilemapPlateform").GetComponent<PlatformEffector2D>().surfaceArc = 180;
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Hurt()
    {
        anim.SetTrigger("Hurt");
        audioS.PlayOneShot(sndHurt);
        pb.Val -= 10;
        if (pb.Val <= 0) Dead();
    }

    public void Win()
    {
        anim.SetTrigger("Win");
        audioS.PlayOneShot(sndWin);
    }

    public void Dead()
    {
        anim.SetTrigger("Dead");
        audioS.PlayOneShot(sndDead);
        GetComponent<KnightScript>().enabled = false;
        StartCoroutine(pause());
    }
    IEnumerator pause()
    {
        yield return new WaitForSeconds(2f);
        canvasGO.SetActive(true);
        StartCoroutine(pause2());
    }
    IEnumerator pause2()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Cowboy":
            case "Globlin":
            case "Skeleton":
            case "Fish":
            case "Mummy":
            

                if (OnAttack)
                {
                    Rigidbody2D rbGoblin = collision.gameObject.GetComponent<Rigidbody2D>();
                    rbGoblin.bodyType = RigidbodyType2D.Dynamic;
                    rbGoblin.AddForce(Vector2.up * 2000);
                    if (collision.gameObject.tag == "Skeleton")
                    {
                        audioS.PlayOneShot(sndSkeleton);
                    }
                    else if (collision.gameObject.tag == "Mummy")
                    {
                        audioS.PlayOneShot(sndMummy);
                    }
                    else
                    {
                        audioS.PlayOneShot(sndGoblin);
                    }
                }
                else
                {
                    PlayerHurtMove(collision);
                }
            break;

            case "Mace":
                if (OnAttack)
                {
                    Rigidbody2D rbMace = collision.gameObject.GetComponent<Rigidbody2D>();
                    rbMace.AddForce(Vector3.right * 6000);
                }
                else
                {
                PlayerHurtMove(collision);
                }
            break;

            case "FishWall":
                Hurt();
                break;

            case "LimitPlayer":
                Dead();
                break;

        }
    }

    void PlayerHurtMove(Collision2D col)
    {
        Vector2 move = col.transform.position - transform.position;
        rb.AddForce(move.normalized * -300);
        Hurt();
    }

    public void AttackBool()
    {
        OnAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            pb.Val = 0;
            canvasGO.SetActive(true);
            StartCoroutine(pause3());
        }
        IEnumerator pause3()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        if (collision.CompareTag("Heart"))
        {
            pb.Val += collision.gameObject.GetComponent<Heart>().valeur;
            audioS.PlayOneShot(sndPickUp);
            Destroy(collision.gameObject,0.2f);
            
            
        }
    }
}
