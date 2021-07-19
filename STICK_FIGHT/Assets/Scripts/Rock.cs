using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    Vector2 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 5);
        transform.position = Vector2.Lerp(transform.position, defaultPos + new Vector2(0, 10), 0.2f);
    }
}
