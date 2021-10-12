using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructeurScripte : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(tag != "player")
        Destroy(collision.transform.parent.gameObject);
    }
}
