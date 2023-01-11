using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleEXP : MonoBehaviour
{
    public int amount = 300;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

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