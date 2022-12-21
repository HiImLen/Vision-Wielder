using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
    public AudioManager audioManager { get; private set; }
    public LevelLoader levelLoader { get; private set; }
    public SaveManager saveManager { get; private set; }
    public GameObject menu;
    public GameObject stopMenu;

    public bool newGame { get; private set;}

    void Awake()
    {
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);

        audioManager = GetComponentInChildren<AudioManager>();
        levelLoader = GetComponentInChildren<LevelLoader>();
        saveManager = GetComponentInChildren<SaveManager>();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
        {
            stopMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        stopMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoToMenu()
    {
        // Save game
        //TimerScript timer = FindObjectOfType<TimerScript>();
        //float time = timer.gameTimer;
        //PlayerBehavior player = FindObjectOfType<PlayerBehavior>();
        //int health = player.currentHealth;
        //saveManager.SaveGameBinary(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, time, health);

        levelLoader.LoadLevel(0);
        audioManager.StopMusic();
        menu.SetActive(true);
        stopMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ContinueGame()
    {
        newGame = false;
        GameSaveData.SaveData data = saveManager.LoadGameBinary();
        levelLoader.LoadLevel(data.level);
    }

    public void NewGame()
    {
        newGame = true;
        levelLoader.LoadLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void ChangeResolution(int value)
    {
        switch (value)
        {
            case 0:
                SetResolution("1920x1080");
                break;
            case 1:
                SetResolution("1600x900");
                break;
            case 2:
                SetResolution("1280x720");
                break;
            default:
                break;
        }
    }

    private void SetResolution(string resolution)
    {
        string[] res = resolution.Split('x');
        Screen.SetResolution(int.Parse(res[0]), int.Parse(res[1]), Screen.fullScreen);
        Debug.Log("Resolution set to " + resolution);
    }
}
