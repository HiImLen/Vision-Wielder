using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float lifeTime = 5.0f;
    [SerializeField] int fireCount = 1; // number of projectiles fired at once
    [SerializeField] float fireDelay = 0.1f;
    [SerializeField] float fireRate = 1.0f;
    [SerializeField] float fireRange = 10.0f;
    
    private GameObject player;
    private float nextFireTime = 0.0f;

    private void Awake()
    {
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (Time.time > nextFireTime)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < fireRange)
            {
                nextFireTime = Time.time + fireRate;
                for (int i = 0; i < fireCount; i++)
                {
                    Invoke("Fire", fireDelay * i);
                }
            }
        }
    }

    private void Fire() {
        if (player == null) return;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<EnemyTrackingProjectile>().Launch(player.transform.position, speed, lifeTime);
        projectile.GetComponent<EnemyTrackingProjectile>().enemyBehavior = GetComponent<EnemyBehavior>();
    }
}
