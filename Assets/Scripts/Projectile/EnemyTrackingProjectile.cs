using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackingProjectile : MonoBehaviour
{
    private float speed;
    private float lifeTime;
    public EnemyBehavior enemyBehavior;
    private Rigidbody2D rigidbody2d;
    Vector3 targetPosition;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    public void Launch(Vector3 targetPosition, float speed, float lifeTime)
    {
        this.targetPosition = targetPosition;
        this.speed = speed;
        this.lifeTime = lifeTime;
        Destroy(gameObject, lifeTime);

        Vector2 direction = (targetPosition - transform.position).normalized;

        // fire projectile once with no tracking with constant speed
        rigidbody2d.velocity = direction * speed;

        //rotate the projectile to face the target
        Vector3 difference = targetPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                StartCoroutine(damageable.Damaged(enemyBehavior.damage, (bool success) => { }));
                Destroy(gameObject);
            }
        }
    }
}
