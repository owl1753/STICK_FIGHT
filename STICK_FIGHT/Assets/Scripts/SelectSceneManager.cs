using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    public Color[] playerColors;
    public Image[] buttonImages;
    public Color selectedButtonColor;
    public Color deselectedButtonColor;
    int selectedButtonIndex = -1;

    void Update()
    {
        if (PlayerPrefs.HasKey("PlayerIndex"))
        {
            selectedButtonIndex = PlayerPrefs.GetInt("PlayerIndex");
        }
    }
    void FixedUpdate()
    {
        if (selectedButtonIndex != -1)
        {
            buttonImages[selectedButtonIndex].color = Color.Lerp(buttonImages[selectedButtonIndex].color, selectedButtonColor, 0.2f);
            for (int i = 0; i < buttonImages.Length; i++)
            {
                if (i == selectedButtonIndex)
                    continue;
                buttonImages[i].color = Color.Lerp(buttonImages[i].color, deselectedButtonColor, 0.2f);
            }
        }  
    }

    public void SetCharactor(int playerIndex)
    {
        PlayerPrefs.SetFloat("R", playerColors[playerIndex].r);
        PlayerPrefs.SetFloat("G", playerColors[playerIndex].g);
        PlayerPrefs.SetFloat("B", playerColors[playerIndex].b);
        PlayerPrefs.SetFloat("A", playerColors[playerIndex].a);
        PlayerPrefs.SetInt("PlayerIndex", playerIndex);
    }

    public void ChooseButtonClick()
    {
        if (PlayerPrefs.HasKey("PlayerIndex"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void PrevButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
