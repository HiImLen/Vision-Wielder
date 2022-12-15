using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class XianglingController : MonoBehaviour
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
            Invoke("NormalAttack", 0.1f);
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
