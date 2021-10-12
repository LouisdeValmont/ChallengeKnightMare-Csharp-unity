using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {


    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<KnightScript>().Hurt();
        }

        Destroy(gameObject);
    }
}
