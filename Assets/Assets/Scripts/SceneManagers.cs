using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1;
    }
    public static void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void quitGame()
    {
        Application.Quit();
    }
}
