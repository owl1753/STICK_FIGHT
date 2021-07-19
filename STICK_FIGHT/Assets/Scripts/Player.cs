using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region ANIMATION
    [Header ("Animation")]
    public Rigidbody2D[] boneRbs;
    public Rigidbody2D[] boneRbs_Attack;
    public Animator copyAnim_Move;
    public Animator copyAnim_Attack;
    public Transform[] copyBones_Move;
    public Transform[] copyBones_Attack;
    public SpriteRenderer[] copySprites_Move;
    public SpriteRenderer[] copySprites_Attack;
    [Range(0, 1)]
    public float copyTreshold;
    public float[] groundAngles;
    [Range(0, 1)]
    public float treshold;
    #endregion

    #region MOVE
    [Header("Move")]
    public Transform body;
    public Rigidbody2D rb;
    public float checkTheGroundDistance;
    public float moveInput;
    public float moveSpeed;
    [Range(0, 1)]
    public float drag;
    public float jumpPower;
    public bool jumpInput;
    public bool isGround;
    #endregion

    #region WEAPON
    [Header ("Weapon")]
    public GameObject[] weapons_L;
    public GameObject[] weapons_R;
    public int weaponIndex;
    GameObject weapon_L;
    GameObject weapon_R;
    #endregion

    #region DEATH
    [Header("Death")]
    public Joint2D[] joints;
    public bool isDead;
    public bool dying;
    public bool isLived;
    public float deadPower;
    #endregion

    #region ATTACK
    enum Direction {RIGHT, LEFT};
    [Header("Attack")]
    public bool attackInput;
    public bool canAttack;
    public bool attacking;
    Direction dir;
    Direction attackDir;
    #endregion

    #region SKILL
    [Header("Skill")]
    public GameObject swordOuraPrefab;
    public GameObject slicing;
    public GameObject rock;
    public bool skillInput;
    public bool canUseSkill;
    public float skillCoolTime;
    public float maxCoolTime;
    public float[] maxCoolTimes;
    public bool usingSkill;
    bool cannotMove;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        foreach (SpriteRenderer sp in copySprites_Move)
        {
            sp.enabled = false;
        }

        foreach (SpriteRenderer sp in copySprites_Attack)
        {
            sp.enabled = false;
        }
         
        weapon_L = weapons_L[weaponIndex];
        weapon_R = weapons_R[weaponIndex];

        isLived = true;
        isDead = false;

        dir = Direction.RIGHT;
        canAttack = true;
        maxCoolTime = maxCoolTimes[weaponIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (moveInput > 0)
                dir = Direction.RIGHT;
            if (moveInput < 0)
                dir = Direction.LEFT;

            int layerMask = (1 << LayerMask.NameToLayer("Ground"));
            RaycastHit2D ray = Physics2D.Raycast(body.position, Vector2.down, Mathf.Infinity, layerMask);
            if (ray.collider != null && ray.collider.CompareTag("Ground") && ray.distance < checkTheGroundDistance)
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }

            if (jumpInput)
            {
                jumpInput = false;
                rb.AddForce(Vector2.up * jumpPower);
            }

            copyAnim_Move.SetBool("Running_R", moveInput > 0);
            copyAnim_Move.SetBool("Running_L", moveInput < 0);
            switch (weaponIndex)
            {
                case 0:
                    {
                        copyAnim_Attack.SetBool("Attacking_1_L", attacking && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Attacking_1_R", attacking && attackDir == Direction.RIGHT);
                        copyAnim_Attack.SetBool("Skill_1_L", usingSkill && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Skill_1_R", usingSkill && attackDir == Direction.RIGHT);
                        break;
                    }
                case 1:
                    {
                        copyAnim_Attack.SetBool("Attacking_2_L", attacking && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Attacking_2_R", attacking && attackDir == Direction.RIGHT);
                        copyAnim_Attack.SetBool("Skill_2_L", usingSkill && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Skill_2_R", usingSkill && attackDir == Direction.RIGHT);
                        break;
                    }
                case 2:
                    {
                        copyAnim_Attack.SetBool("Attacking_3_L", attacking && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Attacking_3_R", attacking && attackDir == Direction.RIGHT);
                        copyAnim_Attack.SetBool("Skill_3_L", usingSkill && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Skill_3_R", usingSkill && attackDir == Direction.RIGHT);
                        break;
                    }
                case 3:
                    {
                        copyAnim_Attack.SetBool("Attacking_4_L", attacking && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Attacking_4_R", attacking && attackDir == Direction.RIGHT);
                        copyAnim_Attack.SetBool("Skill_4_L", usingSkill && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Skill_4_R", usingSkill && attackDir == Direction.RIGHT);
                        break;
                    }
                case 4:
                    {
                        copyAnim_Attack.SetBool("Attacking_5_L", attacking && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Attacking_5_R", attacking && attackDir == Direction.RIGHT);
                        copyAnim_Attack.SetBool("Skill_5_L", usingSkill && attackDir == Direction.LEFT);
                        copyAnim_Attack.SetBool("Skill_5_R", usingSkill && attackDir == Direction.RIGHT);
                        break;
                    }
            }
            if (attackInput && canAttack)
            {
                attackInput = false;
                canAttack = false;
                StartCoroutine(Attack());
            }

            if (skillInput && canUseSkill)
            {
                skillInput = false;
                canUseSkill = false;
                switch (weaponIndex)
                {
                    case 0:
                        {
                            StartCoroutine(Skill1());
                            break;
                        }
                    case 1:
                        {
                            StartCoroutine(Skill2());
                            break;
                        }
                    case 2:
                        {
                            StartCoroutine(Skill3());
                            break;
                        }
                    case 3:
                        {
                            StartCoroutine(Skill4());
                            break;
                        }
                    case 4:
                        {
                            StartCoroutine(Skill5());
                            break;
                        }
                }
            } 
            
            if (!usingSkill && skillCoolTime < maxCoolTime)
            {
                skillCoolTime += Time.deltaTime;
            }

            if (skillCoolTime >= maxCoolTime)
            {
                canUseSkill = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            for (int i = 0; i < boneRbs.Length; i++)
            {
                if (attacking && i == 0)
                {
                    continue;
                }
                if (isGround)
                {
                    boneRbs[i].MoveRotation(Mathf.LerpAngle(boneRbs[i].rotation, copyBones_Move[i].rotation.eulerAngles.z, copyTreshold));
                    groundAngles[i] = boneRbs[i].rotation;
                }
                else
                {
                    boneRbs[i].MoveRotation(Mathf.LerpAngle(boneRbs[i].rotation, groundAngles[i], treshold));
                }
            }
            rb.AddForce(Vector2.right * moveInput * moveSpeed);
        }
        else
        {
            if (!dying)
            {
                StartCoroutine(Die());
            }
        }

        if (attacking)
        {
            for (int i = 0; i < boneRbs_Attack.Length; i++)
            {
                boneRbs_Attack[i].MoveRotation(copyBones_Attack[i].rotation.eulerAngles.z);
            }
        }

        if (usingSkill)
        {
            for (int i = 0; i < boneRbs_Attack.Length; i++)
            {
                boneRbs_Attack[i].MoveRotation(copyBones_Attack[i].rotation.eulerAngles.z);
            }
        }
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, drag), rb.velocity.y);
    }

    IEnumerator Attack()
    {
        attacking = true;
        attackDir = dir;
        GameObject weaponUsed = null;
        switch (dir)
        {
            case Direction.RIGHT:
                {
                    weaponUsed = weapon_R;
                    break;
                }
            case Direction.LEFT:
                {
                    weaponUsed = weapon_L;
                    break;
                }
        }
        yield return new WaitForSeconds(0.4f);
        weaponUsed.SetActive(true);
        switch (weaponIndex)
        {
            case 0:
            case 4:
                {
                    FindObjectOfType<AudioManager>().PlayOneShot("Slash2");
                    break;
                }
            case 1:
                {
                    FindObjectOfType<AudioManager>().PlayOneShot("Slash1");
                    break;
                }
            case 2:
            case 3:
                {
                    FindObjectOfType<AudioManager>().PlayOneShot("Slash3");
                    break;
                }
        }
        yield return new WaitForSeconds(0.6f);
        weaponUsed.SetActive(false);
        attacking = false;
        canAttack = true;
        if (FindObjectOfType<Boss>() != null)
        {
            FindObjectOfType<Boss>().isHit = false;
        }
            
    }

    IEnumerator Skill1()
    {
        usingSkill = true;
        attackDir = dir;
        skillCoolTime = 0;
        GameObject weaponUsed = null;
        switch (dir)
        {
            case Direction.RIGHT:
                {
                    weaponUsed = weapon_R;
                    break;
                }
            case Direction.LEFT:
                {
                    weaponUsed = weapon_L;
                    break;
                }
        }
        weaponUsed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 5; i++)
        {
            GameObject swordOura = Instantiate(swordOuraPrefab, weaponUsed.transform.position, weaponUsed.transform.rotation);
            switch (attackDir)
            {
                case Direction.RIGHT:
                    {
                        swordOura.GetComponent<Rigidbody2D>().velocity = Vector2.right * 20;
                        swordOura.transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
                case Direction.LEFT:
                    {
                        swordOura.GetComponent<Rigidbody2D>().velocity = Vector2.left * 20;
                        swordOura.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    }
            }
            FindObjectOfType<AudioManager>().PlayOneShot("Slash2");
            yield return new WaitForSeconds(1);
        }
        weaponUsed.SetActive(false);
        usingSkill = false;
        if (FindObjectOfType<Boss>() != null)
        {
            FindObjectOfType<Boss>().isHit = false;
        }
        yield return null;
    }

    IEnumerator Skill2()
    {
        usingSkill = true;
        attackDir = dir;
        skillCoolTime = 0;
        GameObject weaponUsed = null;
        switch (dir)
        {
            case Direction.RIGHT:
                {
                    weaponUsed = weapon_R;
                    break;
                }
            case Direction.LEFT:
                {
                    weaponUsed = weapon_L;
                    break;
                }
        }
        weaponUsed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        float timer = 0.05f;
        while (!isGround)
        {
            rb.AddForce(Vector2.down * 1000);
            yield return new WaitForFixedUpdate();
        }
        Instantiate(rock, transform.position - new Vector3(0, 10, 0), Quaternion.Euler(0, 0, 0));
        FindObjectOfType<AudioManager>().PlayOneShot("Rock");
        while (timer > 0)
        {
            rb.AddForce(Vector2.down * 1000);
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.2f);
        weaponUsed.SetActive(false);
        usingSkill = false;
        if (FindObjectOfType<Boss>() != null)
        {
            FindObjectOfType<Boss>().isHit = false;
        }
        yield return null;
    }

    IEnumerator Skill3()
    {
        usingSkill = true;
        attackDir = dir;
        skillCoolTime = 0;
        GameObject weaponUsed = null;
        switch (dir)
        {
            case Direction.RIGHT:
                {
                    weaponUsed = weapon_R;
                    break;
                }
            case Direction.LEFT:
                {
                    weaponUsed = weapon_L;
                    break;
                }
        }
        yield return new WaitForSeconds(0.4f);
        weaponUsed.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        while (weaponUsed.transform.localScale.y < 5)
        {
            weaponUsed.transform.localScale = weaponUsed.transform.localScale + new Vector3(0, Time.deltaTime * 20, 0);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        while (weaponUsed.transform.localScale.y > 1)
        {
            weaponUsed.transform.localScale = weaponUsed.transform.localScale - new Vector3(0, Time.deltaTime * 20, 0);
            yield return null;
        }
        yield return new WaitForSeconds(0.6f);
        weaponUsed.SetActive(false);
        usingSkill = false;
        if (FindObjectOfType<Boss>() != null)
        {
            FindObjectOfType<Boss>().isHit = false;
        }
        yield return null;
    }

    IEnumerator Skill4()
    {
        usingSkill = true;
        attackDir = dir;
        skillCoolTime = 0;
        GameObject weaponUsed = null;
        switch (dir)
        {
            case Direction.RIGHT:
                {
                    weaponUsed = weapon_R;
                    break;
                }
            case Direction.LEFT:
                {
                    weaponUsed = weapon_L;
                    break;
                }
        }
        weaponUsed.SetActive(true);
        FindObjectOfType<AudioManager>().PlayOneShot("Slash3");
        float timer = 0.2f;
        while (timer > 0)
        {
            switch (dir)
            {
                case Direction.RIGHT:
                    {
                        rb.velocity = new Vector2(100, 0);
                        break;
                    }
                case Direction.LEFT:
                    {
                        rb.velocity = new Vector2(-100, 0);
                        break;
                    }
            }
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.3f);
        weaponUsed.SetActive(false);
        usingSkill = false;
        if (FindObjectOfType<Boss>() != null)
        {
            FindObjectOfType<Boss>().isHit = false;
        }
        yield return null;
    }

    IEnumerator Skill5()
    {
        usingSkill = true;
        attackDir = dir;
        skillCoolTime = 0;
        GameObject weaponUsed = null;
        switch (dir)
        {
            case Direction.RIGHT:
                {
                    weaponUsed = weapon_R;
                    break;
                }
            case Direction.LEFT:
                {
                    weaponUsed = weapon_L;
                    break;
                }
        }
        weaponUsed.SetActive(true);
        GameObject sc = Instantiate(slicing, transform.position, transform.rotation);
        Destroy(sc, 5);
        yield return new WaitForSeconds(5);
        weaponUsed.SetActive(false);
        usingSkill = false;
        if (FindObjectOfType<Boss>() != null)
        {
            FindObjectOfType<Boss>().isHit = false;
        }
        yield return null;
    }

    IEnumerator Die()
    {
        dying = true;
        rb.AddForce(Vector2.up * deadPower);
        FindObjectOfType<AudioManager>().PlayOneShot("PlayerDeath");
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].enabled = false;
        }
    }
}
