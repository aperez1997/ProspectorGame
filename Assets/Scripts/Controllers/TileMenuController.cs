using UnityEngine;
using UnityEngine.SceneManagement;

public class TileMenuController : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        player = GameState.Instance.Player;
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

    public void Rest()
    {       
        player.ResetActionPoints();
    }

    public void TestAddItem()
    {
        if (player.HasEnoughActionPoints(1))
        {
            player.UseActionPoints(1);
            player.Inventory.AddItem(new InventoryItem(ItemType.GoldNugget, 1));
        }        
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
