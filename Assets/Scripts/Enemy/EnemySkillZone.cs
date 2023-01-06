using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillZone : MonoBehaviour
{
    public EnemyBehavior enemyBehavior;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                StartCoroutine(damageable.Damaged(Mathf.RoundToInt(enemyBehavior.damage), null));
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    void OnObJectDestroy()
    {
        Destroy(gameObject);
    }
}
