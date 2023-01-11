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
    public GameObject defeatMenu;
    public GameObject winMenu;
    public bool isLoading;
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
        levelLoader.LoadLevel(0);
        audioManager.StopMusic();
        menu.SetActive(true);
        stopMenu.SetActive(false);
        winMenu.SetActive(false);
        Time.timeScale = 1;
    }

        public void GoToMenuWithoutSave()
    {
        levelLoader.LoadLevel(0);
        audioManager.StopMusic();
        menu.SetActive(true);
        defeatMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ContinueGame()
    {
        newGame = false;
        GameSaveData.SaveData data = saveManager.LoadGameBinary();
        levelLoader.LoadLevel(data.level);
    }

    public void RestartGame()
    {
        levelLoader.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void WinGame(int level, int time, int health)
    {
        saveManager.SaveGameBinary(level, time, health);
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameOver(int level, int time, int health)
    {
        saveManager.SaveGameBinary(level, time, health);
        defeatMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void NewGame(int level)
    {
        newGame = true;
        levelLoader.LoadLevel(level);
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
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1600, 900, false);
                break;
            case 2:
                Screen.SetResolution(1280, 700, false);
                break;
            default:
                break;
        }
    }
}
