using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button ContinueButton;

    void Awake()
    {
        ContinueButton.interactable = SaveSystem.HasGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueGame()
    {
        if (GameStateManager.LoadGame())
        {
            LoadOverworld();
        }
    }

   public void CreateNewGame()
    {
        Debug.Log("Creating new game");
        GameStateManager.CreateNewGame();
        LoadOverworld();
    }

    public static void LoadOverworld()
    {
        Debug.Log("Loading overworld");
        SceneManager.LoadScene("Overworld");
    }
}
