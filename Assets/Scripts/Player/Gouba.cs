using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Gouba : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackSpeed = 1f;
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
    }

    void Start()
    {
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1 / attackSpeed && attackCount < 5)
        {
            elapsedTime = 0.0f;
            skillAttack();
        }

        if(attackCount == 5) {
            Destroy(gameObject, 1f);
        }
    }

    private void skillAttack()
    {
        // get closest enemy
        GameObject closestEnemy = enemySpawner.GetClosestEnemy(transform.position);

        if (closestEnemy == null)
            return;

        GameObject goubaProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        goubaProjectile.GetComponent<GoubaProjectileScript>().Launch(closestEnemy.transform.position);
        attackCount++;

        // flip gouba toward closetEnemy
        if (closestEnemy.transform.position.x < transform.position.x)
            spriteRenderer.flipX = true;
        else if (closestEnemy.transform.position.x > transform.position.x)
            spriteRenderer.flipX = false;
    }
}
