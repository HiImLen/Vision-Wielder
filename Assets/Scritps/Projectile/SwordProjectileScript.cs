using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectileScript : MonoBehaviour
{
    [SerializeField] public float speed = 200f;
    [SerializeField] private float lifeTime = 1.5f;
    private PlayerBehavior playerBehavior;
    private Rigidbody2D rigidbody2d;
    
    bool dicrectionUp;
    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerBehavior = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {

    }

    public void Launch(Vector2 targetPosition)
    {
        //fire projectile once with no tracking
        rigidbody2d.AddForce((targetPosition - (Vector2)transform.position).normalized * speed);

        //rotate the projectile to face the target
        Vector2 difference = targetPosition - (Vector2)transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            StartCoroutine(damageable.Damaged(playerBehavior.damage, null));
        }
    }
}
