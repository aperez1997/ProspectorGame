using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Transform ItemContainer;
    public GameObject ItemTemplate;

    private Inventory inventory;

    private void Awake()
    {
        ItemContainer = transform.Find("Panel-Inventory");
    }

    private void Start()
    {
        UpdateInventoryUI();
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory; 
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    public void UpdateInventoryUI()
    {
        // remove old display
        foreach (Transform child in ItemContainer)
        {
            Destroy(child.gameObject);
        }

        // display items   
        foreach (Item item in inventory.ItemList)
        {
            GameObject goItem = Instantiate(ItemTemplate, ItemContainer);
            goItem.SetActive(true);

            // find sprit
            Image image = goItem.GetComponentInChildren<Image>();
            image.sprite = item.GetSprite();

            // find text and adjust
            Text itemAmountText = goItem.GetComponentInChildren<Text>();
            itemAmountText.text = item.amount.ToString();
            itemAmountText.enabled = true;
        }
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        UpdateInventoryUI();
    }

    public void TestItemAdd()
    {
        inventory.AddItem(new Item { amount = 1, type = Item.ItemType.Ration }); 
    }
}
