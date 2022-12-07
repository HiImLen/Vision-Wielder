using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private int _damage = 5;
    public int damage { get {return _damage;} set {damage = _damage;} }
    public int maxHealth { get {return _maxHealth;} set {maxHealth = _maxHealth;} }
    public int currentHealth { get; set; }
    [SerializeField] private float internalHitCD = 2f;
    private HealthBar healthBar;

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
        yield return new WaitForSeconds(internalHitCD);
        callback(true);
    }

    public void OnObjectDestroy() { }
}
