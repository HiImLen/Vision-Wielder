using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float gameTimer = 0.0f;
    private bool isRunning = true;

    private void Awake()
    {
        if (!GameManager.Instance.newGame)
        {
            GameSaveData.SaveData data = GameManager.Instance.saveManager.LoadGameBinary();
            // set game timer to closet to minute mark like 60s, 120s, 180s, etc.
            gameTimer = data.timer;
            gameTimer = Mathf.FloorToInt(gameTimer / 60f) * 60;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            gameTimer += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(gameTimer);
            timerText.text = timeSpan.ToString(@"mm\:ss");
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }
}
