using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    
    public void LoadLevel(int index)
    {
        GameManager.Instance.isLoading = true;
        GameManager.Instance.menu.SetActive(false);
        StartCoroutine(LoadLevelAsync(index));
    }

    IEnumerator LoadLevelAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        loadingScreen.SetActive(true);
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
        GameManager.Instance.isLoading = false;
    }
}
