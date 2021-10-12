using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WesternShootScript : MonoBehaviour {

    [SerializeField] float speed = 2f, shootForce = 800f;
    Vector2 startPos;
    bool lookRight = true;
    SpriteRenderer sp;

    [SerializeField] LayerMask theLayer;
    [SerializeField] float shootDistance = 5f;
    [SerializeField] bool shoot = false;
    Animator anim;
    AudioSource audios;

    [SerializeField] GameObject bullet;

    void Start()
    {
        startPos = transform.position;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audios = GetComponent<AudioSource>();

    }

    void Update()
    {
        if(!shoot)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);

            if (Vector2.Distance(transform.position, startPos) < 0.5f && !lookRight)
                FlipCharacter();
        }

        //IA Fire
        Vector2 rayDir = lookRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, shootDistance, theLayer);
        Debug.DrawRay(transform.position, rayDir * shootDistance);

        if(hit.collider !=null)
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.CompareTag("Player"))
            {
                shoot = true;
                anim.SetBool("shoot", true);
                if (!audios.isPlaying) audios.Play();
            }
        }
        else
        {
            shoot = false;
            anim.SetBool("shoot", false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PointRetour")
        {
            FlipCharacter();
        }
    }

    void FlipCharacter()
    {
        sp.flipX = !sp.flipX;
        speed = -speed;
        lookRight = !lookRight;
    }

    public void ShootBullet()
    {
        GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector2 dir = lookRight ? Vector2.right : Vector2.left;
        b.GetComponent<Rigidbody2D>().AddForce(dir * shootForce);
    }

    
}
