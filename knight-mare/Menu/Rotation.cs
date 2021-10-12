using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] int vitesse = 10;
    
    void Update()
    {
        transform.Rotate(Vector3.forward * vitesse * Time.deltaTime);
    }
}
