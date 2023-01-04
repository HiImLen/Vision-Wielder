using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int maxCapacity;
    public float[] enemySpawnRate;
    public int[] spawnCount;
    public GameObject[] enemyPrefabs;
    public GameObject[] miniBossPrefabs;
    public GameObject[] bossPrefabs;
    public AudioClip[] music;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.audioManager.PlayMusic(music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
