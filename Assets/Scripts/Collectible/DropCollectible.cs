using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCollectible : MonoBehaviour
{
    [SerializeField] private GameObject EXP_Prefab;
    [SerializeField] private GameObject Health_Prefab;
    [SerializeField] private GameObject Magnet_Prefab;
    [SerializeField] private GameObject Bomb_Prefab;
    [SerializeField] private GameObject Energy_Prefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DropEXP()
    {
        Vector3 pos = new Vector3(transform.position.x + 1.0f, transform.position.y + 0.5f, transform.position.z);
        GameObject exp = Instantiate(EXP_Prefab, pos, Quaternion.identity);
    }

    public void DropHealth()
    {
        Vector3 pos = new Vector3(transform.position.x - 1.0f, transform.position.y - 0.5f, transform.position.z);
        GameObject health = Instantiate(Health_Prefab, pos, Quaternion.identity);
    }

    public void DropMagnet()
    {
        GameObject magnet = Instantiate(Magnet_Prefab, transform.position, Quaternion.identity);
    }

    public void DropBomb()
    {
        GameObject bomb = Instantiate(Bomb_Prefab, transform.position, Quaternion.identity);
    }

    public void DropEnergy()
    {
        GameObject energy = Instantiate(Energy_Prefab, transform.position, Quaternion.identity);
    }
}