using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombImpact : MonoBehaviour
{
    private int bombMultiplier = 10;
    private PlayerBehavior playerBehavior;

    void Start()
    {
        playerBehavior = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && !collision.CompareTag("Player"))
        {
            StartCoroutine(damageable.Damaged(Mathf.RoundToInt(playerBehavior.damage * bombMultiplier), null));
        }
        Destroy(gameObject);
    }
}