using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] playerSps;
    public GameObject[] gameObjects;

    // Start is called before the first frame update
    void Start()
    {
        Color color = Color.white;

        if (PlayerPrefs.HasKey("R"))
        {
            color = new Color(
            PlayerPrefs.GetFloat("R"),
            PlayerPrefs.GetFloat("G"),
            PlayerPrefs.GetFloat("B"),
            PlayerPrefs.GetFloat("A"));
        }

        if (PlayerPrefs.HasKey("WeaponNumber"))
        {
            gameObjects[PlayerPrefs.GetInt("WeaponNumber")].SetActive(true);
        }

        foreach (SpriteRenderer sp in playerSps)
        {
            sp.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
