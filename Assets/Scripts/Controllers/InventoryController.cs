using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject ItemTemplate;

    private Transform ItemContainer;

    private Inventory inventory;

    private void Awake()
    {
        ItemContainer = FindInChildren("Item Container").transform;
    }

    private void Start()
    {
        // get inventory from singleton. Is this the right way?
        SetInventory(GameState.Instance.Inventory);
        UpdateInventoryUI();
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        // subscribe to changes.
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
        foreach (InventoryItem item in inventory.ItemList)
        {
            GameObject goItem = Instantiate(ItemTemplate, ItemContainer);
            goItem.SetActive(true);

            // find sprit
            Image image = goItem.GetComponentInChildren<Image>();
            image.sprite = item.Sprite;

            // find amount text and adjust
            Text itemAmountText = goItem.GetComponentInChildren<Text>();
            if (item.Stackable)
            {
                itemAmountText.text = item.amount.ToString();
                itemAmountText.enabled = true;
            } else
            {
                itemAmountText.enabled = false;
            }


            // for tooltips
            ToolTipUIHelper helper = goItem.GetComponent<ToolTipUIHelper>();
            helper.text = item.Name + "\n" + item.Description;
        }
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        UpdateInventoryUI();
    }

    public GameObject FindInChildren(string name)
    {
        foreach (var x in gameObject.GetComponentsInChildren<Transform>())
            if (x.gameObject.name == name)
                return x.gameObject;
        throw new System.Exception("Technically the old version throws an exception if none are found, so I'll do the same here!");
    }
}
