using UnityEngine;

// Responsible for getting GameState instance into correct state at the beginning
public class GameStateController : MonoBehaviour
{
    private void Awake()
    {
        GameState data = SaveSystem.LoadData();
        if (data == null)
        {
            Debug.Log("Creating blank gamedata");
            data = new GameState();
        } else
        {
            Debug.Log("Loaded gamedata");
        }
        GameState.Instance = data;
    }

    public static void SaveGame()
    {
        Debug.Log("Saving gamedata");
        SaveSystem.SaveData(GameState.Instance);
    }
}
