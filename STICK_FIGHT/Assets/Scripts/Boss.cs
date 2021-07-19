using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    enum State { Walk, Attack, Die};
    public Player player;
    public Rigidbody2D rb;
    public Animator anim;
    public Collider2D sword;
    public float moveSpeed;
    public float dashForce;
    public float jumpForce;
    public float jumpAngle;
    public float backStepAngle;
    public float backStepForce;
    public Image healthBar;
    public bool isHit;
    public int maxHp;
    public int hp;
    Vector2 substract;
    float angle;
    State state;
    bool attacking;
    bool dying;
    IEnumerator attackCoroutine;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        sword.enabled = false;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Walking", state == State.Walk);
        anim.SetBool("Ready", state == State.Attack);
        anim.SetBool("Die", state == State.Die);

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, hp / (float)maxHp, 0.4f);
    }

    void FixedUpdate()
    {
        substract = player.transform.position - transform.position;
        if (hp <= 0)
        {
            state = State.Die;
        }
        else if (Mathf.Abs(substract.x) >= 30 && !attacking)
        {
            state = State.Walk;
        }
        else
        {
            state = State.Attack;
        }

        angle = Mathf.Atan2(substract.y, substract.x) * Mathf.Rad2Deg;
        switch (state)
        {
            case State.Walk:
                {
                    if (Mathf.Cos(angle * Mathf.Deg2Rad) > 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        rb.AddForce(Vector2.right * moveSpeed);
                    }
                    if (Mathf.Cos(angle * Mathf.Deg2Rad) < 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        rb.AddForce(Vector2.left * moveSpeed);
                    }
                    break;
                }
            case State.Attack:
                {
                    if (!attacking)
                    {
                        int pattern;
                        if (Mathf.Abs(substract.x) < 10)
                        {
                            pattern = Random.Range(3, 5);
                        }
                        else if (Mathf.Abs(substract.x) < 20)
                        {
                            pattern = Random.Range(0, 1);
                        }
                        else if (Mathf.Abs(substract.x) < 30)
                        {
                            pattern = Random.Range(1, 3);
                        }
                        else
                        {
                            pattern = Random.Range(0, 5);
                        }
                        switch (pattern)
                        {
                            case 0:
                                {
                                    attackCoroutine = Attack1();
                                    break;
                                }
                            case 1:
                                {
                                    attackCoroutine = Attack2();
                                    break;
                                }
                            case 2:
                                {
                                    attackCoroutine = Attack3();
                                    break;
                                }
                            case 3:
                                {
                                    attackCoroutine = Attack4();
                                    break;
                                }
                            case 4:
                                {
                                    attackCoroutine = Attack5();
                                    break;
                                }
                        }
                        
                        StartCoroutine(attackCoroutine);
                        if (Mathf.Cos(angle * Mathf.Deg2Rad) > 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        if (Mathf.Cos(angle * Mathf.Deg2Rad) < 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }

                    break;
                }
            case State.Die:
                {
                    if (attackCoroutine != null)
                    {
                        StopCoroutine(attackCoroutine);
                        sword.enabled = false;
                        attacking = false;
                        canvas.SetActive(false);
                    }
                    if (!dying)
                    {
                        StartCoroutine(Die());
                    }
                    break;
                }
        }
    }

    IEnumerator Attack1()
    {
        attacking = true;
        yield return new WaitForSeconds(0.1f);
        if (transform.rotation.eulerAngles.y == 0)
        {
            rb.AddForce(Vector2.right * dashForce);
        }
        if (transform.rotation.eulerAngles.y == 180)
        {
            rb.AddForce(Vector2.left * dashForce);
        }
        anim.SetBool("Attacking1", true);
        yield return new WaitForSeconds(1);
        sword.enabled = true;
        FindObjectOfType<AudioManager>().PlayOneShot("BossSlash");
        yield return new WaitForSeconds(1);
        sword.enabled = false;
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attacking1", false);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    IEnumerator Attack2()
    {
        attacking = true;
        yield return new WaitForSeconds(0.1f);
        if (transform.rotation.eulerAngles.y == 0)
        {
            rb.AddForce(new Vector2(Mathf.Cos(jumpAngle * Mathf.Deg2Rad), Mathf.Sin(jumpAngle * Mathf.Deg2Rad)) * jumpForce);
        }
        if (transform.rotation.eulerAngles.y == 180)
        {
            rb.AddForce(new Vector2(Mathf.Cos((180 - jumpAngle) * Mathf.Deg2Rad), Mathf.Sin((180 - jumpAngle) * Mathf.Deg2Rad)) * jumpForce);
        }
        anim.SetBool("Attacking2-1", true);
        yield return new WaitForSeconds(2.3f);
        anim.SetBool("Attacking2-1", false);
        anim.SetBool("Attacking2-2", true);
        FindObjectOfType<AudioManager>().PlayOneShot("BossSlash");
        sword.enabled = true;
        yield return new WaitForSeconds(0.5f);
        sword.enabled = false;
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attacking2-2", false);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    IEnumerator Attack3()
    {
        attacking = true;
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Attacking3", true);
        yield return new WaitForSeconds(0.4f);
        if (transform.rotation.eulerAngles.y == 0)
        {
            rb.AddForce(Vector2.right * dashForce * 2);
        }
        if (transform.rotation.eulerAngles.y == 180)
        {
            rb.AddForce(Vector2.left * dashForce * 2);
        }
        
        sword.enabled = true;
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<AudioManager>().PlayOneShot("BossSlash");
        yield return new WaitForSeconds(0.5f);
        sword.enabled = false;
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Attacking3", false);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    IEnumerator Attack4()
    {
        attacking = true;
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Attacking4", true);
        yield return new WaitForSeconds(1.8f);
        FindObjectOfType<AudioManager>().PlayOneShot("BossSlash");
        sword.enabled = true;
        yield return new WaitForSeconds(0.5f);
        sword.enabled = false;
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("Attacking4", false);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    IEnumerator Attack5()
    {
        attacking = true;
        yield return new WaitForSeconds(0.1f);
        if (transform.rotation.eulerAngles.y == 0)
        {
            rb.AddForce(new Vector2(Mathf.Cos(backStepAngle * Mathf.Deg2Rad), Mathf.Sin(backStepAngle * Mathf.Deg2Rad)) * backStepForce);
        }
        if (transform.rotation.eulerAngles.y == 180)
        {
            rb.AddForce(new Vector2(Mathf.Cos((180 - backStepAngle) * Mathf.Deg2Rad), Mathf.Sin((180 - backStepAngle) * Mathf.Deg2Rad)) * backStepForce);
        }
        anim.SetBool("Attacking5", true);
        yield return new WaitForSeconds(1.2f);
        anim.SetBool("Attacking5", false);
        attacking = false;
    }

    IEnumerator Die()
    {
        dying = true;
        yield return new WaitForSeconds(5);
        FindObjectOfType<GameManager>().MainMenu();
    }

    void OnTriggerEnter2D(Collider2D cd)
    {
        if (cd.CompareTag("Weapon") && !isHit)
        {
            isHit = true;
            hp -= 1;
        }
    }
}
