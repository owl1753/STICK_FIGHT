using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicing : MonoBehaviour
{
    Player player;
    float maxTimer = 0.2f;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            FindObjectOfType<AudioManager>().PlayOneShot("Slash2");
            timer = maxTimer;
        }
    }
}
