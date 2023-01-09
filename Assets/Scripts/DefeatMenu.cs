using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class DefeatMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI levelText;
    private TimeSpan timeSpan;

    void OnEnable()
    {
        if (timeText != null && levelText != null)
        {
            TimerScript timer = FindObjectOfType<TimerScript>();
            timeSpan = TimeSpan.FromSeconds(timer.gameTimer);
            timeText.text = timeSpan.ToString(@"mm\:ss");
            levelText.text = "Level " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }
    }
}
