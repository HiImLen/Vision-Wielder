using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private int _damage = 5;
    [SerializeField] public float internalHitCD = 2f;
    public int damage { get {return _damage;} set {damage = _damage;} }
    public int maxHealth { get {return _maxHealth;} set {maxHealth = _maxHealth;} }
    public int currentHealth { get; set; }
    private HealthBar healthBar;
    public float normalAttackMultiplier = 0.42f;
    public float skillMultiplier = 1.11f;
    public float burstMultiplier = 1.12f;

    void Start()
    {
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // if (isInvincible)
        // {
        //     invincibleTimer -= Time.deltaTime;
        //     if (invincibleTimer <= 0)
        //         isInvincible = false;
        // }
    }

    public IEnumerator Damaged(int damage, System.Action<bool> callback)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        HitParticle();
        if (currentHealth <= 0)
        {
            // Die
            GameManager.Instance.GameOver();
        }
        yield return new WaitForSeconds(internalHitCD);
        callback(true);
    }

    private void HitParticle()
    {
        GameObject particle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particle, 1f);
    }

    public void OnObjectDestroy() { }
}
