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

        SceneManager.sceneLoaded += OnSceneLoaded_NewGame;

        LoadOverworld();
    }

    public void OnSceneLoaded_NewGame(Scene scene, LoadSceneMode mode)
    {
        // TODO: this text should come from an SO somewhere
        GameStateManager.LogicInstance.ShowModal("Welcome", "Some txt would go here about you trying to make a fortune and get back to your wife.");
        SceneManager.sceneLoaded -= OnSceneLoaded_NewGame;
    }

    public static void LoadOverworld()
    {
        Debug.Log("Loading overworld");
        SceneManager.LoadScene("Overworld");
    }
}
