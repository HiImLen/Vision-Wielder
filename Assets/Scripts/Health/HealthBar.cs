using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private Vector3 offset;
    [SerializeField] private TextMeshProUGUI healthText;
    string maxHealth;

    void Awake()
    {
        
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        maxHealth = health.ToString();
        
        fill.color = gradient.Evaluate(1f);
        if (healthText != null) healthText.text = maxHealth + " / " + maxHealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
        if (healthText != null) healthText.text = health.ToString() + " / " + maxHealth;
    }
}
