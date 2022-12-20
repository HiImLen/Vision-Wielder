using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class YoimiyaController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int projectileCount = 1;
    private EnemySpawner enemySpawner;
    private float elapsedTime = 0.0f;
    public float moveSpeed = 5f;
    Rigidbody2D rb;
    Vector2 movementInput;
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
        if (elapsedTime >= 1/attackSpeed)
        {
            elapsedTime = 0.0f;
            for (int i = 0; i < projectileCount; i++)
            {
                Invoke("NormalAttack", 0.1f * i);
            }
        }
    }

    private void NormalAttack()
    {
        if(enemySpawner.enemyList.Count == 0) return;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    void OnFixedUpdate()
    {
    }

    void OnSkill()
    {

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
