using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class YoimiyaController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private int projectileCount = 1;
    [SerializeField] private float skillCDTimer = 18.0f; // skill cooldown timer
    [SerializeField] private float skillEffectTimer = 10.0f; // skill effect timer
    [SerializeField] private float burstCDTimer = 20.0f; // burst cooldown timer

    private EnemySpawner enemySpawner;
    private float elapsedTime = 0.0f;
    private PlayerBehavior playerBehavior;
    public float moveSpeed = 5f;

    Rigidbody2D rb;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Vector2 direction = new Vector2(1, 0);

    float skillCD; // skill cooldown
    bool isSkillCD = false;

    float burstCD; // burst cooldown
    bool isBurstCD = false;

    float skillEffectCD; // skill effect cooldown

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        playerBehavior = GetComponent<PlayerBehavior>();
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        skillCD = skillCDTimer;
        burstCD = burstCDTimer;
        skillEffectCD = skillEffectTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Attack speed: " + attackSpeed);
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1 / attackSpeed)
        {
            elapsedTime = 0.0f;
            for (int i = 0; i < projectileCount; i++)
            {
                Invoke("NormalAttack", 0.1f * i);
            }
        }

        if (isSkillCD)
        {
            skillCD -= Time.deltaTime;
            if (skillCD <= 0)
            {
                skillCD = skillCDTimer;
                isSkillCD = false;
            }

            if (skillEffectCD <= 0)
            {
                skillEffectCD = skillEffectTimer;
                attackSpeed -= playerBehavior.normalAttackMultiplier * 3;
                playerBehavior.skillMultiplier /= 2.0f;
            }
            if (skillEffectCD < skillEffectTimer)
                skillEffectCD -= Time.deltaTime;
        }

        if (isBurstCD)
        {
            burstCD -= Time.deltaTime;
            if (burstCD <= 0)
            {
                burstCD = burstCDTimer;
                isBurstCD = false;
            }
        }
    }

    private void NormalAttack()
    {
        if (enemySpawner.enemyList.Count == 0) return;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    void OnFixedUpdate()
    {

    }

    void OnSkill()
    {
        if (skillCD == skillCDTimer)
        {
            attackSpeed += playerBehavior.normalAttackMultiplier * 3;
            playerBehavior.skillMultiplier *= 2.0f;
            isSkillCD = true;
            skillEffectCD -= Time.deltaTime;
        }
    }

    void OnBurst()
    {
        if (burstCD == burstCDTimer)
        {
            // Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            isBurstCD = true;
        }
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            // set isMoving to true
        }
        if (movementInput.x < 0)
            spriteRenderer.flipX = true;
        else if (movementInput.x > 0)
            spriteRenderer.flipX = false;
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
}
