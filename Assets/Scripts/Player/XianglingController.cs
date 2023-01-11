using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class XianglingController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int projectileCount = 1;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float skillCDTimer = 12.0f;
    [SerializeField] private float burstCDTimer = 20.0f;

    [SerializeField] private GameObject goubaPrefab;
    [SerializeField] private GameObject burstPrefab;

    private SpriteRenderer spriteRenderer;
    private EnemySpawner enemySpawner;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 direction = new Vector2(1, 0);

    private float skillCD, burstCD;
    private bool isSkillCD = false, isBurstCD = false;
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
        skillCD = skillCDTimer;
        burstCD = burstCDTimer;
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1 / attackSpeed)
        {
            elapsedTime = 0.0f;
            Invoke("NormalAttack", 0.1f);
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

    private void checkSkill()
    {
        if (isSkillCD) 
        {
            skillCD -= Time.deltaTime;
            if (skillCD <= 0) {
                skillCD = skillCDTimer;
                isSkillCD = false;
            }
        }
    }

    private void checkBurst()
    {
        if (isBurstCD) 
        {
            burstCD -= Time.deltaTime;
            if (burstCD <= 0) {
                burstCD = burstCDTimer;
                isBurstCD = false;
            }
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
            GameObject gouba = Instantiate(goubaPrefab, transform.position, Quaternion.identity);
            isSkillCD = true;
        }
    }

    void OnBurst(){
        if (burstCD == burstCDTimer) 
        {
            GameObject burst = Instantiate(burstPrefab, transform.position, Quaternion.identity);
            isBurstCD = true;
        }
    }

    private void NormalAttack()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            // get mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            if (mousePosition.x < transform.position.x)
                spriteRenderer.flipX = true;
            else if (mousePosition.x > transform.position.x)
                spriteRenderer.flipX = false;

            // rotate mouse position around player position by angle
            float angle = 30f * (i - (projectileCount - 1) / 2f);
            mousePosition += Quaternion.Euler(0, 0, angle) * (mousePosition - transform.position);

            GameObject swordProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            swordProjectile.GetComponent<SwordProjectileScript>().Launch(mousePosition);
        }
    }
}
