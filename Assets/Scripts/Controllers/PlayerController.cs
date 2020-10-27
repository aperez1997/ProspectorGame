using System;
using UnityEngine;
using UnityEngine.UI;

// Actions related stuffs
public class PlayerController : MonoBehaviour
{
    protected Player player;

    public Text healthPointTxt;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameState.Instance.Player;
        player.OnHealthChanged += Player_OnHealthChanged;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActionRest()
    {
        player.Sleep();
    }

    public void ActionPanForGold()
    {
        if (player.HasEnoughActionPoints(1))
        {
            player.UseActionPoints(1);
            player.Inventory.AddItem(new InventoryItem(ItemType.GoldNugget, 1));
        }
    }

    void UpdateHealthUI()
    {
        healthPointTxt.text = player.Health.ToString() + "/" + Player.MAX_HEALTH.ToString();
    }

    private void Player_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthUI();
    }
}
