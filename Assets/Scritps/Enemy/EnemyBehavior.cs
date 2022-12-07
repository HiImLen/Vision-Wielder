using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private int _damage = 20;
    public int damage { get { return _damage; } set { damage = _damage; } }
    public int maxHealth { get { return _maxHealth; } set { maxHealth = _maxHealth; } }
    public int currentHealth { get; set; }
    private Transform player;
    private Rigidbody2D rigidbody2d;
    private Vector2 movement;
    public EnemySpawner enemySpawner;
    public float speed = 1f;
    Animator animator;
    SpriteRenderer spriteRenderer;
    private bool hitAble = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        movement = (player.position - transform.position).normalized; // normalized to get a unit vector of the direction
    }

    void FixedUpdate() // Physics update
    {
        Animation();
        MoveTowardPlayer();
    }

    void Animation()
    {
        // Flip the sprite if the direction is left
        if (movement.x < 0)
            spriteRenderer.flipX = true;
        else if (movement.x > 0)
            spriteRenderer.flipX = false;
    }

    void MoveTowardPlayer()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + movement * speed * Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (hitAble)
            {
                IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    hitAble = false;
                    StartCoroutine(damageable.Damaged(damage, (bool success) => { hitAble = success; }));
                }
            }
        }
    }

    public IEnumerator Damaged(int damage, System.Action<bool> callback)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            enemySpawner.DeleteEnemy(gameObject);
        yield return null;
    }

    public void OnObjectDestroy()
    {
        enemySpawner.DeleteEnemy(gameObject);
    }
}
