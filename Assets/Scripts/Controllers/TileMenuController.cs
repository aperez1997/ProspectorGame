using UnityEngine;

public class TileMenuController : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        player = GameData.Instance.Player;
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
        player.Inventory.AddItem(new Item { amount = 1, type = Item.ItemType.Ration });
    }
}
