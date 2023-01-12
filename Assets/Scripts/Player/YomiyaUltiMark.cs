using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YomiyaUltiMark : MonoBehaviour
{
    // The distance at which the raycast should check for enemies
    private float radius = 5.0f;

    // The layer mask for the enemy layer
    public GameObject burstMark;

    private LayerMask enemyLayer;
    private GameObject markPrefab;
    private IDamageable damageable;
    private PlayerBehavior playerBehavior;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        markPrefab = Resources.Load<GameObject>("AimSprite");
        damageable = GetComponent<IDamageable>();
        playerBehavior = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        StartCoroutine(OnBurstHit());
    }

    // Update is called once per frame
    void Update()
    {
        checkMark();
    }

    public void UseMarkSkill()
    {
        // int count = 0;
        Vector2 enemyPos = transform.position;

        // Cast a circle around the enemy to find nearby enemies
        Collider2D[] hits = Physics2D.OverlapCircleAll(enemyPos, radius, enemyLayer);

        // Iterate over the hits and apply the "mark" effect to any enemies
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.GetComponent<IDamageable>() != null && hit.gameObject.GetComponent<YomiyaUltiMark>() == null)
            {
                hit.gameObject.AddComponent<YomiyaUltiMark>();
            }
        }
    }

    void checkMark()
    {
        if (burstMark != null)
        {
            burstMark.transform.position = transform.position;
        }

        if (transform.GetComponent<YomiyaUltiMark>() != null && burstMark == null)
        {
            burstMark = Instantiate(markPrefab, transform.position, Quaternion.identity);
        }
    }

    IEnumerator OnBurstHit()
    {
        while (true)
        {
            StartCoroutine(damageable.Damaged(Mathf.RoundToInt(playerBehavior.damage * playerBehavior.burstMultiplier), null));
            yield return new WaitForSeconds(2.0f);
        }
    }
}