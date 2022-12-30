using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowProjectileScript : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] private float lifeTime = 5f;
    private PlayerBehavior playerBehavior;
    private Rigidbody2D rigidbody2d;
    Vector3 targetPosition;
    private EnemySpawner enemySpawner;
    private GameObject closestEnemy;
    bool targetAcquired = false;

    bool dicrectionUp;
    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerBehavior = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        dicrectionUp = Random.Range(0, 2) == 0;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // fire projectile with speed up and tracking
        // Vector3 directionToTarget = targetPosition - transform.position;
        // Vector3 currentDirection = transform.forward;
        // float maxTurnSpeed = 120f; // degrees per second
        // Vector3 resultingDirection = Vector3.RotateTowards(currentDirection, directionToTarget, maxTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1f);
        // Debug.Log(resultingDirection);
        // transform.rotation = Quaternion.LookRotation(resultingDirection);
        // rigidbody2d.AddForce(transform.forward * speed);

        if (enemySpawner.enemyList.Count != 0 && targetAcquired == false)
        {
            closestEnemy = enemySpawner.GetClosestEnemy(transform.position);
            targetAcquired = true;
        }
    }

    private void FixedUpdate()
    {
        if (closestEnemy != null)
        {
            // fire projectile with tracking, fire randomly upward or downward
            Vector2 dicrection = (closestEnemy.transform.position - transform.position).normalized;
            float rotateAmount = Vector3.Cross(dicrection, (dicrectionUp ? transform.up : -transform.up)).z;
            rigidbody2d.angularVelocity = -rotateAmount * Random.Range(00, 1400);
            rigidbody2d.velocity = (dicrectionUp ? transform.up : -transform.up) * speed;
        }
    }

    public void Launch(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;

        // fire projectile once with no tracking
        //rigidbody2d.AddForce((targetPosition - transform.position).normalized * speed);

        // rotate the projectile to face the target
        //Vector3 difference = targetPosition - transform.position;
        //float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            StartCoroutine(damageable.Damaged(Mathf.RoundToInt(playerBehavior.damage * playerBehavior.skillMultiplier), null));
            Destroy(gameObject);
        }
    }
}
