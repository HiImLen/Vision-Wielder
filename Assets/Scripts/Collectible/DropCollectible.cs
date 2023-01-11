using UnityEngine;
using UnityEngine.SceneManagement;

public class DropCollectible : MonoBehaviour
{
    [SerializeField] private GameObject Health_Prefab;
    [SerializeField] private GameObject Bomb_Prefab;

    public void DropHealth()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), transform.position.z);
        GameObject health = Instantiate(Health_Prefab, pos, Quaternion.identity);
    }

    public void DropBomb()
    {
        GameObject bomb = Instantiate(Bomb_Prefab, transform.position, Quaternion.identity);
    }

    void OnDestroy()
    {
        if (GameManager.Instance.isLoading) return;
        
        if (CompareTag("Boss"))
        {
            DropHealth();
            DropBomb();
            if (Random.Range(0, 100) < 50)
            {
                DropHealth();
            }
            else if (Random.Range(0, 100) < 50)
            {
                DropBomb();
            }
        }
        if (CompareTag("MiniBoss"))
        {
            DropHealth();
            DropBomb();
        }
        else // If normal enemy
        {
            if (Random.Range(0, 100) < 1)
            {
                DropHealth();
            }
            else if (Random.Range(0, 100) < 1)
            {
                DropBomb();
            }
        }
    }
}