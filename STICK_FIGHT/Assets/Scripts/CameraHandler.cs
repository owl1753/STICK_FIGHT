using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Player player;
    public float delay;
    public float cameraPosY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.isDead == false)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, cameraPosY, -10), delay);
        }
    }
}
