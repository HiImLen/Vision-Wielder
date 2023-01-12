using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleEXP : MonoBehaviour
{
    [SerializeField] private float amount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
        if (player != null)
        {
            player.AddExp(amount);
            Destroy(gameObject);
        }
    }
}
