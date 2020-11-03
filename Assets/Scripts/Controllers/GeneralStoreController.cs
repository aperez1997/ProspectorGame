using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralStoreController : MonoBehaviour
{
    public Button ExitButton;

    public void ExitStore()
    {
        SceneManager.UnloadSceneAsync("GeneralStore");
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Overworld"));
    }
}
