using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 50;
    [SerializeField] private int _damage = 20;
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private GameObject healthTextprefab;
    [SerializeField] private GameObject bossHealthBar;
    [SerializeField] private string bossName;
    private GameObject healthBarCanvas;
    private HealthBar healthBar;
    public int damage { get { return _damage; } set { damage = _damage; } }
    public int maxHealth { get { return _maxHealth; } set { maxHealth = _maxHealth; } }
    public int currentHealth { get; set; }
    public GameObject player;
    public Rigidbody2D rigidbody2d;
    public EnemySpawner enemySpawner;
    public float speed = 1f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool hitAble = true;
    private bool isDead = false;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBarCanvas = Instantiate(bossHealthBar, transform.position, Quaternion.identity);
        healthBar = healthBarCanvas.GetComponentInChildren<HealthBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Set the name of the boss
        //healthBarCanvas.GetComponentInChildren<TextMeshProUGUI>().text = bossName;


        // Set attack state base on the name of the boss
        BossStateManager stateManager = GetComponent<BossStateManager>();

        if (bossName == "Andrius")
        {
            stateManager.attackState = new AndriusAttackState();
        }
        else if (bossName == "Dvalin")
        {
            stateManager.attackState = new DvalinAttackState();
        }
        else if (bossName == "Ruin Hunter")
        {
            stateManager.attackState = new RuinHunterAttackState();
        }
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
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                DropCollectible dc = GetComponent<DropCollectible>();

                for (int i = 0; i < 3; i++)
                {
                    dc.DropEXP();
                }
                dc.DropHealth();
                dc.DropBomb();

                //animator.SetTrigger("Die");
                speed = 0;
                gameObject.GetComponent<Collider2D>().enabled = false;
                OnObjectDestroy();
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
        Destroy(healthBarCanvas);
    }
}
