using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XianglingUltiProjectileScript : MonoBehaviour
{
    [SerializeField] private float lifeTime = 12.0f;

    private float angularSpeed, rotationRadius;
    private PlayerBehavior playerBehavior;
    private Rigidbody2D rigidbody2d;
    private Transform rotationCenter;

    private float posX, posY, angle;

    bool dicrectionUp;
    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerBehavior = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        rotationCenter = GameObject.FindWithTag("Player").GetComponent<Transform>();
        angularSpeed = -4.5f;
        rotationRadius = 3.0f;
        posX = posY = angle = 0f;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        Launch();
    }

    private void Launch()
    {
        // Burst Movement
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * angularSpeed;
        // Debug.Log("posX: " + posX + " posY: " + posY);
        // Debug.Log("angle: " + angle);
        // Debug.Log("angularSpeed: " + angularSpeed);

        if (angle >= 360f)
            angle = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            // Debug.Log("Burst Hit Enemy!");
            //other.gameObject.GetComponent<Rigidbody2D>().AddForce((other.gameObject.transform.position - transform.position).normalized * 0.05f);
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                StartCoroutine(damageable.Damaged(Mathf.RoundToInt(playerBehavior.damage * playerBehavior.burstMultiplier), null));
            }
        }
    }
}
