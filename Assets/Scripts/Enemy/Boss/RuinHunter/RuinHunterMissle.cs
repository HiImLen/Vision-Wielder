using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinHunterMissle : MonoBehaviour
{
    private float speed;
    private float lifeTime;
    public BossBehavior bossBehavior;
    private Rigidbody2D rigidbody2d;
    Vector3 targetPosition;
    Vector2 direction;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 newPosition = rigidbody2d.position + direction * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(newPosition);
    }

    public void Launch(Vector3 targetPosition, float speed, float lifeTime)
    {
        this.targetPosition = targetPosition;
        this.speed = speed;
        this.lifeTime = lifeTime;
        Destroy(gameObject, lifeTime);

        direction = (targetPosition - transform.position).normalized;

        //rotate the projectile to face the target
        Vector3 difference = targetPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                StartCoroutine(damageable.Damaged(bossBehavior.damage, (bool success) => { }));
                Destroy(gameObject);
            }
        }
    }
}
