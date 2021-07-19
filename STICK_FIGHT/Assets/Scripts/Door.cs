using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public SpriteRenderer doorSp;
    public Collider2D doorCd;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectsOfType<Enemy>().Length > 0)
        {
            doorSp.enabled = false;
            doorCd.enabled = false;
        }
        else
        {
            doorSp.enabled = true;
            doorCd.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D cd)
    {
        if (cd.CompareTag("PlayerBone"))
        {
            StartCoroutine(FindObjectOfType<GameManager>().LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
}
