using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private int _damage = 20;
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private GameObject healthTextprefab;
    public int damage { get { return _damage; } set { damage = _damage; } }
    public int maxHealth { get { return _maxHealth; } set { maxHealth = _maxHealth; } }
    public int currentHealth { get; set; }

    private Transform player;
    private Rigidbody2D rigidbody2d;
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

    }

    void FixedUpdate() // Physics update
    {
        Animation();
        MoveTowardPlayer();
    }

    void Animation()
    {
        // Flip the enemy if the direction is left
        if (transform.position.x > player.position.x)
            //spriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (transform.position.x < player.position.x)
            //spriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void MoveTowardPlayer()
    {
        // move toward player at constant speed
        Vector2 position = transform.position;
        position = Vector2.MoveTowards(position, player.position, speed * Time.deltaTime);
        rigidbody2d.MovePosition(position);
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
        //AudioManager.Instance.PlayEffect(hitSound);
        StartCoroutine(ChangeColor());
        HitParticle();
        ShowDamageText(damage);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                DropCollectible dc = GetComponent<DropCollectible>();

                if (CompareTag("MiniBoss"))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        dc.DropEXP();
                    }
                    dc.DropHealth();
                    dc.DropBomb();
                }
                else // If normal enemy
                {
                    if (Random.Range(0, 1000) < 2)
                    {
                        dc.DropHealth();
                    }
                    else if (Random.Range(0, 1000) < 2)
                    {
                        dc.DropBomb();
                    }
                    else dc.DropEXP();
                }
                animator.SetTrigger("Die");
                speed = 0;
                isDead = true;
            }
        }
        yield return null;
    }

    private void HitParticle()
    {
        GameObject particle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particle, 1f);
    }

    public IEnumerator ChangeColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void ShowDamageText(int damage)
    {
        GameObject damageText = Instantiate(healthTextprefab, transform.position, Quaternion.identity);
        damageText.GetComponent<TextMeshPro>().text = damage.ToString();
        //damageText.GetComponent<TextMeshPro>().color = Color.red;
        damageText.GetComponent<MeshRenderer>().sortingOrder = 2;

        damageText.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        Destroy(damageText, 1f);
    }

    public void OnObjectDestroy()
    {
        enemySpawner.DeleteEnemy(gameObject);
    }
}