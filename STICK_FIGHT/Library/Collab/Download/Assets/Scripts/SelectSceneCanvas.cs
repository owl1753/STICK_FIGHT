using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSceneCanvas : MonoBehaviour
{
    public Color[] playerColors;

    void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnClickButton(Transform tr)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == tr.GetSiblingIndex())
            {
                PlayerPrefs.SetFloat("R", playerColors[i].r);
                PlayerPrefs.SetFloat("G", playerColors[i].g);
                PlayerPrefs.SetFloat("B", playerColors[i].b);
                PlayerPrefs.SetFloat("A", playerColors[i].a);
                PlayerPrefs.SetInt("WeaponNumber", i);
            }
        }
        SceneManager.LoadScene("TestScene");
    }
}
