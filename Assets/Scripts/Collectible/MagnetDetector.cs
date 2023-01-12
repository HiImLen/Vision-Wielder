using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagnetDetector : MonoBehaviour
{
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Collectible"))
        {
            collision.gameObject.AddComponent<CollectibleMoving>();
        }
        Destroy(gameObject);
    }
}