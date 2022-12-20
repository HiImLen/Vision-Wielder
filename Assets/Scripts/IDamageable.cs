using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This interface is used to make the enemy take damage
public interface IDamageable
{
    public int damage { get; set; }
    public int maxHealth { get; set; }
    public int currentHealth { get; set; }
    public IEnumerator Damaged(int damage, System.Action<bool> callback);
    public void OnObjectDestroy();
}

