using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float radius;
    [SerializeField] private float rotation;
    public BossBehavior bossBehavior;
    private Rigidbody2D rb;
    private Vector2 desPosition;
    private Vector2 force;
    private bool hitAble = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        desPosition = rb.position + Random.insideUnitCircle * radius;
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction to the black hole
        Vector2 direction = desPosition - rb.position;

        // Rotate the direction by 60 degrees around the Z axis
        direction = Quaternion.Euler(0, 0, rotation) * direction;

        // Calculate the force based on the direction and the speed
        force = direction.normalized * speed;
    }

    void FixedUpdate()
    {
        // Apply the force to the rigidbody
        rb.AddForce(force);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (hitAble)
            {
                IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    hitAble = false;
                    StartCoroutine(damageable.Damaged(Mathf.RoundToInt(bossBehavior.damage), (bool success) => { hitAble = success; }));
                }
            }
        }
    }
}
