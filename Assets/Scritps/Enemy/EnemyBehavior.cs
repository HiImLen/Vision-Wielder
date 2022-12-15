using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private int _damage = 20;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private GameObject healthTextprefab;
    [SerializeField] private AudioClip hitSound;
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
    private bool isDead = false;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
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
        // Flip the enemy if the direction is left
        if (movement.x < 0)
            spriteRenderer.flipX = true;
            //transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (movement.x > 0)
            spriteRenderer.flipX = false;
            //transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void MoveTowardPlayer()
    {
        if (isDead) rigidbody2d.velocity = Vector2.zero;
        else
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
        StartCoroutine(ChangeColor());
        //AudioManager.Instance.PlayEffect(hitSound);
        GameObject particle = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        Destroy(particle, 1f);
        ShowDamageText(damage);
        currentHealth -= damage;
        if (currentHealth <= 0) {
            animator.SetTrigger("Die");
        }
        yield return null;
    }
    
    public IEnumerator ChangeColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void ShowDamageText(int damage)
    {
        GameObject damageText = Instantiate(healthTextprefab, transform.position, Quaternion.identity, transform);
        damageText.GetComponent<TextMeshPro>().text = damage.ToString();
        damageText.GetComponent<MeshRenderer>().sortingOrder = 2;

        damageText.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        Destroy(damageText, 1f);
    }

    public void OnObjectDestroy()
    {
        isDead = true;
        enemySpawner.DeleteEnemy(gameObject);
    }
}
