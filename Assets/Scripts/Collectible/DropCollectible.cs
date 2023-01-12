using UnityEngine;
using UnityEngine.SceneManagement;

public class DropCollectible : MonoBehaviour
{
    [SerializeField] private GameObject EXP_Prefab;
    [SerializeField] private GameObject EXP2_Prefab;
    [SerializeField] private GameObject EXP3_Prefab;
    [SerializeField] private GameObject Health_Prefab;
    [SerializeField] private GameObject Bomb_Prefab;
    [SerializeField] private GameObject Magnet_Prefab;

    public void DropEXP()
    {
        if (EXP_Prefab != null && EXP2_Prefab != null && EXP3_Prefab != null)
        {
            int randomNumber = Random.Range(0, 100);
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
            if (randomNumber < 60)
            {
                Instantiate(EXP_Prefab, spawnPosition, Quaternion.identity);
            }
            else if (randomNumber < 90)
            {
                Instantiate(EXP2_Prefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(EXP3_Prefab, spawnPosition, Quaternion.identity);
            }
            return;
        }
        else if (EXP_Prefab != null && EXP2_Prefab != null || EXP_Prefab != null && EXP3_Prefab != null || EXP2_Prefab != null && EXP3_Prefab != null)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
            if (EXP_Prefab != null && EXP2_Prefab != null)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 70)
                {
                    Instantiate(EXP_Prefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(EXP2_Prefab, spawnPosition, Quaternion.identity);
                }
                return;
            }
            else if (EXP_Prefab != null && EXP3_Prefab != null)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 90)
                {
                    Instantiate(EXP_Prefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(EXP3_Prefab, spawnPosition, Quaternion.identity);
                }
                return;
            }
            else if (EXP2_Prefab != null && EXP3_Prefab != null)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 80)
                {
                    Instantiate(EXP2_Prefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(EXP3_Prefab, spawnPosition, Quaternion.identity);
                }
                return;
            }
        }
        else
        {
            if (EXP_Prefab != null)
            {
                Instantiate(EXP_Prefab, transform.position, Quaternion.identity);
            }
            else if (EXP2_Prefab != null)
            {
                Instantiate(EXP2_Prefab, transform.position, Quaternion.identity);
            }
            else if (EXP3_Prefab != null)
            {
                Instantiate(EXP3_Prefab, transform.position, Quaternion.identity);
            }
        }
    }

        public void DropEXP(Vector3 spawnPosition)
    {
        if (EXP_Prefab != null && EXP2_Prefab != null && EXP3_Prefab != null)
        {
            int randomNumber = Random.Range(0, 100);
            if (randomNumber < 60)
            {
                Instantiate(EXP_Prefab, spawnPosition, Quaternion.identity);
            }
            else if (randomNumber < 90)
            {
                Instantiate(EXP2_Prefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(EXP3_Prefab, spawnPosition, Quaternion.identity);
            }
            return;
        }
        else if (EXP_Prefab != null && EXP2_Prefab != null || EXP_Prefab != null && EXP3_Prefab != null || EXP2_Prefab != null && EXP3_Prefab != null)
        {
            if (EXP_Prefab != null && EXP2_Prefab != null)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 70)
                {
                    Instantiate(EXP_Prefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(EXP2_Prefab, spawnPosition, Quaternion.identity);
                }
                return;
            }
            else if (EXP_Prefab != null && EXP3_Prefab != null)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 80)
                {
                    Instantiate(EXP_Prefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(EXP3_Prefab, spawnPosition, Quaternion.identity);
                }
                return;
            }
            else if (EXP2_Prefab != null && EXP3_Prefab != null)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber < 60)
                {
                    Instantiate(EXP2_Prefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Instantiate(EXP3_Prefab, spawnPosition, Quaternion.identity);
                }
                return;
            }
        }
        else
        {
            if (EXP_Prefab != null)
            {
                Instantiate(EXP_Prefab, spawnPosition, Quaternion.identity);
            }
            else if (EXP2_Prefab != null)
            {
                Instantiate(EXP2_Prefab, spawnPosition, Quaternion.identity);
            }
            else if (EXP3_Prefab != null)
            {
                Instantiate(EXP3_Prefab, spawnPosition, Quaternion.identity);
            }
        }
    }


    public void DropHealth()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
        GameObject health = Instantiate(Health_Prefab, pos, Quaternion.identity);
    }

    public void DropBomb()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
        GameObject bomb = Instantiate(Bomb_Prefab, pos, Quaternion.identity);
    }

    public void DropMagnet()
    {
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
        GameObject magnet = Instantiate(Magnet_Prefab, pos, Quaternion.identity);
    }
}