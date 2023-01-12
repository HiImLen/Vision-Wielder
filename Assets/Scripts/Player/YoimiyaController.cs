using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class YoimiyaController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private int projectileCount = 1;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float skillCDTimer = 18.0f;
    [SerializeField] private float burstCDTimer = 20.0f;
    [SerializeField] private float skillEffectTimer = 10.0f;
    [SerializeField] private float burstEffectTimer = 10.0f;

    private PlayerBehavior playerBehavior;
    private SpriteRenderer spriteRenderer;
    private EnemySpawner enemySpawner;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 direction = new Vector2(1, 0);

    private float skillCD, burstCD;
    private bool isSkillCD = false, isBurstCD = false;
    private float skillEffectCD, burstEffectCD;
    private float elapsedTime = 0.0f;
    private SkillsCD skillsCD;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        skillsCD = GameObject.FindWithTag("SkillsCD").GetComponent<SkillsCD>();
        skillsCD.skillCD = skillCDTimer;
        skillsCD.burstCD = burstCDTimer;
    }
    void Start()
    {
        playerBehavior = GetComponent<PlayerBehavior>();
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        skillCD = skillCDTimer;
        burstCD = burstCDTimer;
        skillEffectCD = skillEffectTimer;
        burstEffectCD = burstEffectTimer;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1 / attackSpeed)
        {
            elapsedTime = 0.0f;
            for (int i = 0; i < projectileCount; i++)
            {
                Invoke("NormalAttack", 0.1f * i);
            }
        }
        checkSkill();
        checkBurst();
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

    private void NormalAttack()
    {
        if (enemySpawner.enemyList.Count == 0) return;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    void checkSkill()
    {
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
                attackSpeed /= 2;
                playerBehavior.normalAttackMultiplier /= playerBehavior.skillMultiplier;
            }
            else if (skillEffectCD < skillEffectTimer)
                skillEffectCD -= Time.deltaTime;
        }
    }

    void checkBurst()
    {
        if (isBurstCD)
        {
            burstCD -= Time.deltaTime;
            if (burstCD <= 0)
            {
                burstCD = burstCDTimer;
                isBurstCD = false;
            }

            if (burstEffectCD <= 0)
            {
                burstEffectCD = burstEffectTimer;
                foreach (GameObject enemy in enemySpawner.enemyList)
                {
                    if (enemy.GetComponent<YomiyaUltiMark>() != null)
                    {
                        Destroy(enemy.GetComponent<YomiyaUltiMark>().burstMark);
                        Destroy(enemy.GetComponent<YomiyaUltiMark>());
                    }
                }
            }
            else if (burstEffectCD < burstEffectTimer)
                burstEffectCD -= Time.deltaTime;
        }
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnSkill()
    {
        if (skillCD == skillCDTimer)
        {
            attackSpeed *= 2;
            playerBehavior.normalAttackMultiplier *= playerBehavior.skillMultiplier;
            isSkillCD = true;
            skillEffectCD -= Time.deltaTime;
        }
    }

    void OnBurst()
    {
        if (burstCD == burstCDTimer)
        {
            GameObject closetEnemy = enemySpawner.GetClosestEnemy(transform.position);
            if (closetEnemy != null)
            {
                closetEnemy.AddComponent<YomiyaUltiMark>();
                burstEffectCD -= Time.deltaTime;
                isBurstCD = true;
            }
        }
    }

    public void IncreaseProjectileCount(int projectileCount)
    {
        this.projectileCount = projectileCount;
    }
}
