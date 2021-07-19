using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Player player;
    public SpriteRenderer[] playerSps;
    public RectTransform[] rectTransforms;
    public float offset;
    public Image fadeImage;
    public Image pauseImage;
    public Image coolTimeImage;
    public float fadeSpeed;
    public GameObject controller;
    public GameObject pauseWindow;
    bool canJump;
    bool canPause = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerIndex"))
        {
            player.weaponIndex = PlayerPrefs.GetInt("PlayerIndex");
            foreach (SpriteRenderer sp in playerSps)
            {
                sp.color = new Color(
                    PlayerPrefs.GetFloat("R"),
                    PlayerPrefs.GetFloat("G"),
                    PlayerPrefs.GetFloat("B"),
                    PlayerPrefs.GetFloat("A")
                    );
            }
        }
        canJump = true;
    }

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("MainTheme");
        StartCoroutine(ThisSceneFadeIn());
        coolTimeImage.color = new Color(PlayerPrefs.GetFloat("R"), PlayerPrefs.GetFloat("G"), PlayerPrefs.GetFloat("B"), coolTimeImage.color.a);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead == true)
        {
            StartCoroutine(Revive());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        coolTimeImage.fillAmount = Mathf.Lerp(coolTimeImage.fillAmount, player.skillCoolTime / player.maxCoolTime, 0.4f);
       
    }

    public void LeftDown()
    {
        player.moveInput = -1;
    }

    public void RightDown()
    {
        player.moveInput = 1;
    }

    public void Up()
    {
        player.moveInput = 0;
    }

    public void JumpClick()
    {
        if (player.isGround && canJump)
        {
            player.jumpInput = true;
            StartCoroutine(JumpDelay());
        }
    }

    public void AttackClick()
    {
        if (player.canAttack && !player.usingSkill)
        {
            player.attackInput = true;
        }   
    }

    public void SkillClick()
    {
        if (player.canUseSkill && !player.attacking)
        {
            player.skillInput = true;
        }
    }

    public void ResumeClick()
    {
        Resume();
    }

    public void RestartClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator JumpDelay()
    {
        canJump = false;
        yield return new WaitForSeconds(0.2f);
        canJump = true;
    }

    IEnumerator ThisSceneFadeIn()
    {
        float fadeImageColorA = fadeImage.color.a;
        canPause = false;
        while(fadeImage.color.a >= 0)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImageColorA);
            fadeImageColorA -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        canPause = true;
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        float fadeImageColorA = fadeImage.color.a;
        canPause = false;
        while (fadeImage.color.a <= 1)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImageColorA);
            fadeImageColorA += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        if (!canPause)
            return;
        Time.timeScale = 0;
        pauseImage.color = new Color(pauseImage.color.r, pauseImage.color.g, pauseImage.color.b, 0.6f);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        player.moveInput = 0;
        controller.SetActive(false);
        pauseWindow.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        pauseImage.color = new Color(pauseImage.color.r, pauseImage.color.g, pauseImage.color.b, 0);
        pauseWindow.SetActive(false);
        StartCoroutine(LoadScene(0));
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pauseImage.color = new Color(pauseImage.color.r, pauseImage.color.g, pauseImage.color.b, 0);
        controller.SetActive(true);
        pauseWindow.SetActive(false);
    }
}
