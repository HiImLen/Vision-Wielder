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
        enemyBehavior = GetComponent<EnemyBehavior>();
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

        // fire projectile once with no tracking
        rigidbody2d.AddForce((targetPosition - transform.position).normalized * speed);

        //rotate the projectile to face the target
        Vector3 difference = targetPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            StartCoroutine(damageable.Damaged(enemyBehavior.damage, null));
            Destroy(gameObject);
        }
    }
}