using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCollectible : MonoBehaviour
{
    [SerializeField] private GameObject Health_Prefab;
    [SerializeField] private GameObject Bomb_Prefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DropHealth()
    {
        Vector3 pos = new Vector3(transform.position.x - 1.0f, transform.position.y - 0.5f, transform.position.z);
        GameObject health = Instantiate(Health_Prefab, pos, Quaternion.identity);
    }

    public void DropBomb()
    {
        GameObject bomb = Instantiate(Bomb_Prefab, transform.position, Quaternion.identity);
    }

    void OnDestroy()
    {
        if (CompareTag("Boss"))
        {
            if (Random.Range(0, 100) < 50)
            {
                DropHealth();
            }
            else if (Random.Range(0, 100) < 50)
            {
                DropBomb();
            }
        }
        if (CompareTag("MiniBoss") || CompareTag("Boss"))
        {
            DropHealth();
            DropBomb();
        }
        else // If normal enemy
        {
            if (Random.Range(0, 100) < 2)
            {
                DropHealth();
            }
            else if (Random.Range(0, 100) < 2)
            {
                DropBomb();
            }
        }
    }
}