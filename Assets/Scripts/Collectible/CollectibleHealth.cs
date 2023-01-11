using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    [SerializeField] private GameObject healthTextprefab;
    [SerializeField] private int amount = 200;

    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBehavior player = collision.GetComponent<PlayerBehavior>();
        if (player != null)
        {
            player.IncreaseHealth(amount);
            ShowHealText(amount);
            Destroy(gameObject);
        }
    }

    private void ShowHealText(int amount)
    {
        GameObject amountText = Instantiate(healthTextprefab, transform.position, Quaternion.identity);
        amountText.GetComponent<TextMeshPro>().text = amount.ToString();
        amountText.GetComponent<MeshRenderer>().sortingOrder = 10;

        amountText.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        Destroy(amountText, 1f);
    }
}