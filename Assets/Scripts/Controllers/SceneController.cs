using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    public static void LoadGeneralStore_Static()
    {
        LoadAdditiveScene("GeneralStore");
    }

    private static void LoadAdditiveScene(string sceneName)
    {
        Debug.Log("Loading scene " + sceneName);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        op.completed += (AsyncOperation o) => {
            Debug.Log("Setting " + sceneName + " active");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        };
    }

    public static void UnloadAdditiveScene()
    {
        var previousSceneIdx = SceneManager.sceneCount - 2;
        var previousScene = SceneManager.GetSceneAt(previousSceneIdx);
        Debug.Log("Previous scene " + previousScene.name);

        var currentScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        Debug.Log("Current scene " + currentScene.name);

        var op = SceneManager.UnloadSceneAsync(currentScene);
        op.completed += (AsyncOperation o) => {
            Debug.Log("Setting scene " + previousScene.name + " active");
            SceneManager.SetActiveScene(previousScene);
        };
    }
}
