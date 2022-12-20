using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetName : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    TextMeshProUGUI text;
    
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText(parent.name);
    }
}
