using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "New Mark", menuName = "Mark")]
public class YomiyaUltiMark : MonoBehaviour
{
    // The distance at which the raycast should check for enemies
    public float radius = 5.0f;

    // The layer mask for the enemy layer
    public LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

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
                // count++;
            }
        }
        // Debug.Log("Marked " + count + " enemies");
    }
}
