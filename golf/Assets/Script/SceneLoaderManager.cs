using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderManager : MonoBehaviour
{
    // Load 
    public static void Load(string sceneName)
    {
        SceneLoader.Load(sceneName);
    }
    // Progress Load 
    public static void ProgressLoad(string sceneName)
    {
        SceneLoader.ProgressLoad(sceneName);
    }
    // Reload Level
    public static void ReloadLevel()
    {
        SceneLoader.ReloadLevel();
    }
    // LoadingLevel
    public static void LoadNextLevel()
    {
        SceneLoader.LoadNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
