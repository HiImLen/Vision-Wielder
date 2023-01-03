using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Gouba : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackSpeed = 1f;
    // [SerializeField] private int projectileCount = 1;
    [SerializeField] private int attackCount = 0;
    private EnemySpawner enemySpawner;
    private float elapsedTime = 0.0f;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Vector2 direction = new Vector2(1, 0);

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log("Gouba is awake");
    }

    void Start()
    {
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        //Debug.Log("Gouba is started");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1 / attackSpeed && attackCount < 5)
        {
            elapsedTime = 0.0f;
            // Invoke("skillAttack", 0.1f);
            skillAttack();
        }

        if(attackCount == 5) {
            Destroy(gameObject, 1f);
        }

        //Debug.Log(Time.deltaTime);
    }

    private void FixedUpdate()
    {

    }

    private void skillAttack()
    {
        // for (int i = 0; i < projectileCount; i++)
        // {
        //     // get mouse position
        //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //     if (mousePosition.x < transform.position.x)
        //         spriteRenderer.flipX = true;
        //     else if (mousePosition.x > transform.position.x)
        //         spriteRenderer.flipX = false;

        //     // rotate mouse position around player position by angle
        //     float angle = 30f * (i - (projectileCount - 1) / 2f);
        //     mousePosition += Quaternion.Euler(0, 0, angle) * (mousePosition - transform.position);
        // }
        
        // get closest enemy
        GameObject closestEnemy = enemySpawner.GetClosestEnemy(transform.position);
        //Debug.Log("CLOSEST ENEMY: " + closestEnemy.transform.position);

        GameObject goubaProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        goubaProjectile.GetComponent<GoubaProjectileScript>().Launch(closestEnemy.transform.position);
        attackCount++;

        //Debug.Log("ATTACK COUNT: " + attackCount);
        // Debug.Log("Gouba is attacking");
    }
}
