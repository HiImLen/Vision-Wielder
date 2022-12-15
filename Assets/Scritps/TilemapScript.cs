using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{
    // Create a new tilemap if the player move outside the bounds of the tilemap
    // This will be used to procedurally generate the tilemap
    private GameObject player;
    private GameObject grassTilemap;
    [SerializeField] GameObject grassTileMapPrefab;
    private Bounds bounds;
    private float offsetX, offsetY = 0;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        grassTilemap = GameObject.FindWithTag("GrassTilemap");
        grassTilemap.GetComponent<Tilemap>().CompressBounds();
        bounds = grassTilemap.GetComponent<Tilemap>().localBounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < (bounds.min.x + offsetX) / 2 ||
            player.transform.position.y < (bounds.min.y + offsetY) / 2 ||
            player.transform.position.x > (bounds.max.x + offsetX) / 2 ||
            player.transform.position.y > (bounds.max.y + offsetY) / 2)
        {
            Vector3 spawnPosition;
            if (player.transform.position.x < (bounds.min.x + offsetX) / 2 || player.transform.position.x > (bounds.max.x + offsetX) / 2)
            {
                offsetX = Mathf.Round(player.transform.position.x);
                bounds.center = new Vector3(offsetX, bounds.center.y, 0);
                spawnPosition = new Vector3(offsetX, bounds.center.y, 0);
            }
            else
            {
                offsetY = Mathf.Round(player.transform.position.y);
                bounds.center = new Vector3(bounds.center.x, offsetY, 0);
                spawnPosition = new Vector3(bounds.center.x, offsetY, 0);
            }
            // Create a new tilemap
            GameObject newTilemap = Instantiate(grassTileMapPrefab, spawnPosition, Quaternion.identity, grassTilemap.transform.parent);
            newTilemap.tag = "GrassTilemap";
            newTilemap.name = "GrassTilemap";

            // Destroy the old tilemap
            Destroy(grassTilemap);

            // Set the new tilemap to the grassTilemap variable
            grassTilemap = newTilemap;
        }
    }
}
