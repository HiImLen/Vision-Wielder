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
        PlayerBehavior player = collision.gameObject.GetComponent<PlayerBehavior>();
        if (damageable != null && player == null)
        {
            StartCoroutine(damageable.Damaged(Mathf.RoundToInt(playerBehavior.damage * playerBehavior.normalAttackMultiplier * bombMultiplier), null));
        }
        Destroy(gameObject);
    }
}