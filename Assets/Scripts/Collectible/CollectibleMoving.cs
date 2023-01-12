using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleMoving : MonoBehaviour
{
    private PlayerBehavior playerBehavior;
    private int moveSpeed = 10;

    void Start()
    {
        playerBehavior = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerBehavior.transform.position, moveSpeed * Time.deltaTime);
    }
}
