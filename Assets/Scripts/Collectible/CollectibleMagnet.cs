using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleMagnet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
        if (player != null)
        {
            //player.IncreaseHealth(amount);
            Destroy(gameObject);
        }
    }
}
