using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oura : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    void Update()
    {
        rb.MoveRotation(rb.rotation - speed);
    }

    void OnTriggerEnter2D(Collider2D cd)
    {
        if (cd.CompareTag("Ground") || cd.CompareTag("Wall") || cd.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
