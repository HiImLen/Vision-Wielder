using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoubaProjectileScript : MonoBehaviour
{
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
    }

    private void Update()
    {

    }

    public void Launch(Vector2 targetPosition)
    {
        //fire projectile once with no tracking
        //rigidbody2d.AddForce((targetPosition - (Vector2)transform.position).normalized * 200f);

        // rotate the projectile to face the target
        Vector2 difference = targetPosition - (Vector2)transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Gouba Hit Enemy!");
        //other.gameObject.GetComponent<Rigidbody2D>().AddForce((other.gameObject.transform.position - transform.position).normalized * 0.05f);
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            StartCoroutine(damageable.Damaged(Mathf.RoundToInt(playerBehavior.damage * playerBehavior.skillMultiplier), null));
        }
    }

    void OnObjectDestroy()
    {
        Destroy(gameObject);
    }
}
