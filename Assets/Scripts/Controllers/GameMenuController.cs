using UnityEngine;
using UnityEngine.SceneManagement;

// Game menu is for loading, saving, quiting, etc
public class GameMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show() { gameObject.SetActive(true); }
    public void Hide() { gameObject.SetActive(false); }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


    public void SaveGame()
    {
        SaveSystem.SaveData(GameState.Instance);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
