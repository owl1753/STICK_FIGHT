using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Common
    public enum Kind { Bullet, DeadLine, Needle, Blade, Extra};
    public Kind kind;
    public float speed;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (kind == Kind.Bullet || kind == Kind.Needle)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)).normalized * speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D cd)
    {
        switch (kind)
        {
            case Kind.Bullet:
                {
                    if (cd.CompareTag("PlayerBone") || cd.CompareTag("Ground"))
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
        }
        if (cd.CompareTag("PlayerBone"))
        {
            FindObjectOfType<Player>().isDead = true;
        }
    }
}
