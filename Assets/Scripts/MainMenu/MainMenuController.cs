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
            LoadMainScene();
        }
    }

   public void CreateNewGame()
    {       
        GameStateManager.CreateNewGame();
        LoadMainScene();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
