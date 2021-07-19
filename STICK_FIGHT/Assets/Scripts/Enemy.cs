using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State { Walk, Attack, Die, Idle };
    public enum Kind { Gun, Sword, Mutant};

    public Kind kind;
    public GameObject[] weapons;
    public Rigidbody2D rb;
    public Player player;
    public float drag;

    Vector2 defaultPos;
    public Transform detectGroundPoint;
    public float moveSpeed;
    public float moveRadius;
    RaycastHit2D detectGroundHit;

    public Transform body;
    public Transform ditectionPoint;
    public Gun gun;
    public LineRenderer laser;
    public float attackRadius;
    public float treshold;
    float defaultDistance;
    bool attacking;
    bool playerDetection;
    IEnumerator attackCoroutine;
    Vector2 subtract;

    public SpriteRenderer[] enemySps;
    public GameObject blood;
    bool isDead;
    bool dying;

    State state;
    public Animator anim;

    public Collider2D sword;

    bool noFriction;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
        state = State.Idle;
        detectGroundHit = Physics2D.Raycast(detectGroundPoint.position, Vector2.down, Mathf.Infinity, (1 << LayerMask.NameToLayer("Ground")));
        defaultDistance = detectGroundHit.distance;

        switch (kind)
        {
            case Kind.Gun:
                {
                    weapons[0].SetActive(true);
                    break;
                }
            case Kind.Sword:
                {
                    weapons[1].SetActive(true);
                    break;
                }
        }
        sword.enabled = false;
    }

    void LateUpdate()
    {
        anim.SetBool("Idle", state == State.Idle);
        anim.SetBool("Walking", state == State.Walk);
        anim.SetBool("Attacking", state == State.Attack);
        anim.SetBool("Dying", state == State.Die);
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!noFriction)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, drag), rb.velocity.y);
        }

        subtract = player.transform.position - transform.position;
        Vector2 attackDir = subtract.normalized;
        float preRadius = subtract.magnitude;

        int layerMask1 = (1 << LayerMask.NameToLayer("PlayerBone")) + (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Ceiling"));
        RaycastHit2D ray = Physics2D.Raycast(ditectionPoint.position, attackDir, Mathf.Infinity, layerMask1);
        int layerMask2 = (1 << LayerMask.NameToLayer("Ground"));
        detectGroundHit = Physics2D.Raycast(detectGroundPoint.position, Vector2.down, Mathf.Infinity, layerMask2);

        if (ray.collider != null && ray.collider.CompareTag("PlayerBone") && player.isDead == false)
        {
            playerDetection = true;
        }
        else
        {
            playerDetection = false;
        }

        switch (kind)
        {
            case Kind.Gun:
                {
                    if (isDead == true)
                    {
                        state = State.Die;
                    }
                    else if (preRadius <= attackRadius && playerDetection)
                    {
                        state = State.Attack;
                    }
                    else if (preRadius <= moveRadius && playerDetection && Mathf.Abs(detectGroundHit.distance - defaultDistance) <= 0.2f)
                    {
                        state = State.Walk;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                    break;
                }
            case Kind.Sword:
                {
                    if (isDead == true)
                    {
                        state = State.Die;
                    }
                    else if ((preRadius <= attackRadius && playerDetection) || attacking)
                    {
                        state = State.Attack;
                    }
                    else if (preRadius <= moveRadius && playerDetection && Mathf.Abs(detectGroundHit.distance - defaultDistance) <= 0.2f)
                    {
                        state = State.Walk;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                    break;
                }
            case Kind.Mutant:
                {
                    if (isDead == true)
                    {
                        state = State.Die;
                    }
                    else if ((preRadius <= attackRadius && playerDetection) || attacking)
                    {
                        state = State.Attack;
                    }
                    else if (preRadius <= moveRadius && playerDetection && Mathf.Abs(detectGroundHit.distance - defaultDistance) <= 0.2f)
                    {
                        state = State.Walk;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                    break;
                }
        }

        switch (state)
        {
            case State.Idle:
                {
                    if (preRadius <= moveRadius && playerDetection)
                    {
                        if (subtract.x > 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            body.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if (subtract.x < 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            body.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    switch (kind)
                    {
                        case Kind.Gun:
                            {
                                if (attackCoroutine != null)
                                {
                                    StopCoroutine(attackCoroutine);
                                    attacking = false;
                                }
                                laser.enabled = false;
                                body.rotation = Quaternion.Euler(body.rotation.eulerAngles.x, body.rotation.eulerAngles.y, Mathf.LerpAngle(body.rotation.eulerAngles.z, 0, treshold));
                                break;
                            }
                    }
                    break;
                }
            case State.Walk:
                {


                    switch (kind)
                    {
                        case Kind.Gun:
                            {
                                if (attackCoroutine != null)
                                {
                                    StopCoroutine(attackCoroutine);
                                    attacking = false;
                                }
                                laser.enabled = false;

                                if (!attacking)
                                {
                                    if (subtract.x > 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 0, 0);
                                        body.rotation = Quaternion.Euler(0, 0, 0);
                                        rb.AddForce(Vector2.right * moveSpeed);
                                    }
                                    else if (subtract.x < 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 180, 0);
                                        body.rotation = Quaternion.Euler(0, 180, 0);
                                        rb.AddForce(Vector2.left * moveSpeed);
                                    }
                                }
                                break;
                            }
                        case Kind.Sword:
                            {
                                if (!attacking)
                                {
                                    if (subtract.x > 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 0, 0);
                                        body.rotation = Quaternion.Euler(0, 0, 0);
                                        rb.AddForce(Vector2.right * moveSpeed);
                                    }
                                    else if (subtract.x < 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 180, 0);
                                        body.rotation = Quaternion.Euler(0, 180, 0);
                                        rb.AddForce(Vector2.left * moveSpeed);
                                    }
                                }
                                break;
                            }
                        case Kind.Mutant:
                            {
                                if (!attacking)
                                {
                                    if (subtract.x > 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 0, 0);
                                        rb.AddForce(Vector2.right * moveSpeed);
                                    }
                                    else if (subtract.x < 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 180, 0);
                                        rb.AddForce(Vector2.left * moveSpeed);
                                    }
                                }
                                break;
                            }
                    }
                    break;
                }
            case State.Attack:
                {
                    float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
                    switch (kind)
                    {
                        case Kind.Gun:
                            {
                                if (Mathf.Cos(angle * Mathf.Deg2Rad) > 0)
                                {
                                    body.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(body.rotation.eulerAngles.z, angle, treshold * 10 * Time.deltaTime));
                                    transform.rotation = Quaternion.Euler(0, 0, 0);
                                }
                                else if (Mathf.Cos(angle * Mathf.Deg2Rad) < 0)
                                {
                                    body.rotation = Quaternion.Euler(0, 180, Mathf.LerpAngle(body.rotation.eulerAngles.z, -angle + 180, treshold * 10 * Time.deltaTime));
                                    transform.rotation = Quaternion.Euler(0, 180, 0);
                                }
                                if (!attacking)
                                {
                                    attackCoroutine = Attack_Gun();
                                    StartCoroutine(attackCoroutine);
                                }
                                break;
                            }
                        case Kind.Sword:
                            {
                                if (!attacking)
                                {
                                    attackCoroutine = Attack_Sword(transform.position.x);
                                    StartCoroutine(attackCoroutine);

                                    if (Mathf.Cos(angle * Mathf.Deg2Rad) > 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 0, 0);
                                    }
                                    else if (Mathf.Cos(angle * Mathf.Deg2Rad) < 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 180, 0);
                                    }
                                }
                                break;
                            }
                        case Kind.Mutant:
                            {
                                if (!attacking)
                                {
                                    attackCoroutine = Attack_Mutant();
                                    StartCoroutine(attackCoroutine);

                                    if (Mathf.Cos(angle * Mathf.Deg2Rad) > 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 0, 0);
                                    }
                                    else if (Mathf.Cos(angle * Mathf.Deg2Rad) < 0)
                                    {
                                        transform.rotation = Quaternion.Euler(0, 180, 0);
                                    }
                                }
                                break;
                            }
                    }
                    break;
                }
            case State.Die:
                {
                    switch (kind)
                    {
                        case Kind.Gun:
                            {
                                if (attackCoroutine != null)
                                {
                                    StopCoroutine(attackCoroutine);
                                    attacking = false;
                                    laser.enabled = false;
                                }
                                break;
                            }
                        case Kind.Sword:
                            {
                                if (attackCoroutine != null)
                                {
                                    StopCoroutine(attackCoroutine);
                                    attacking = false;
                                    sword.enabled = false;
                                }
                                break;
                            }
                        case Kind.Mutant:
                            {
                                if (attackCoroutine != null)
                                {
                                    StopCoroutine(attackCoroutine);
                                    attacking = false;
                                    noFriction = false;
                                }
                                break;
                            }

                    }
                    if (!dying)
                    {
                        StartCoroutine(Die());
                    }
                    break;
                }
        }
    }

    IEnumerator Attack_Gun()
    {
        laser.enabled = true;
        attacking = true;
        yield return new WaitForSeconds(3);
        laser.enabled = false;
        gun.Shoot();
        FindObjectOfType<AudioManager>().PlayOneShot("GunShot");
        yield return new WaitForSeconds(1);
        attacking = false;
    }

    IEnumerator Attack_Sword(float defaultPos)
    {
        attacking = true;
        yield return new WaitForSeconds(1);
        anim.SetBool("Attacking2", true);
        sword.enabled = true;
        while (Mathf.Abs(defaultPos - transform.position.x) <= 10 && Mathf.Abs(detectGroundHit.distance - defaultDistance) <= 0.2f)
        {
            if (transform.rotation.eulerAngles.y == 0)
            {
                transform.position = new Vector2(transform.position.x + Time.deltaTime * 20, transform.position.y);
            }
            if (transform.rotation.eulerAngles.y == 180)
            {
                transform.position = new Vector2(transform.position.x - Time.deltaTime * 20, transform.position.y);
            }
            yield return null;
        }
        FindObjectOfType<AudioManager>().PlayOneShot("EnemySlash");
        yield return new WaitForSeconds(0.3f);
        sword.enabled = false;
        yield return new WaitForSeconds(1);
        anim.SetBool("Attacking2", false);
        attacking = false;
        
    }

    IEnumerator Attack_Mutant()
    {
        attacking = true;
        yield return new WaitForSeconds(1);
        FindObjectOfType<AudioManager>().PlayOneShot("Shout");
        anim.SetBool("Attacking2", true);
        noFriction = true;
        if (transform.rotation.eulerAngles.y == 0)
        {
            rb.AddForce(new Vector2(Mathf.Cos(30 * Mathf.Deg2Rad), Mathf.Sin(30 * Mathf.Deg2Rad)) * 1000);
        }
        if (transform.rotation.eulerAngles.y == 180)
        {
            rb.AddForce(new Vector2(Mathf.Cos(150 * Mathf.Deg2Rad), Mathf.Sin(150 * Mathf.Deg2Rad)) * 1000);
        }
        yield return new WaitForSeconds(1.5f);
        noFriction = false;
        anim.SetBool("Attacking2", false);
        yield return new WaitForSeconds(1.5f);
        
        attacking = false;
    }

    void OnTriggerEnter2D(Collider2D cd)
    {
        switch (kind)
        {
            case Kind.Mutant:
                {
                    if (cd.CompareTag("PlayerBone") && noFriction && attacking)
                    {
                        FindObjectOfType<Player>().isDead = true;
                    }
                    break;
                }
        }

        if (cd.CompareTag("Weapon"))
        {
            isDead = true;
        }
    }

    IEnumerator Die()
    {
        dying = true;
        blood.SetActive(true);
        FindObjectOfType<AudioManager>().PlayOneShot("EnemyHit");
        yield return new WaitForSeconds(3);
        while (enemySps[enemySps.Length - 1].color.a > 0.1f)
        {
            for (int i = 0; i < enemySps.Length; i++)
            {
                if (enemySps[i].color.a <= 0)
                {
                    break;
                }
                enemySps[i].color = new Color(enemySps[i].color.r, enemySps[i].color.g, enemySps[i].color.b,
                    Mathf.Lerp(enemySps[i].color.a, 0, 0.1f));
            }
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
