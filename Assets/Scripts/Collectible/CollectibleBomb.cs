using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleBomb : MonoBehaviour
{
    [SerializeField] private GameObject bombImpactPrefab;

    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
        if (player != null)
        {
            Instantiate(bombImpactPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}