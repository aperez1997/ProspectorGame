using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject ItemTemplate;

    protected Transform ItemContainer;

    protected Inventory inventory;

    protected GameLogic gameLogic;

    private void Awake()
    {
        ItemContainer = Utils.FindInChildren(gameObject, "Item Container").transform;
    }

    private void Start()
    {
        gameLogic = GameState.Instance.GameLogic;

        ItemTemplate.SetActive(false);

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
            // don't destroy the Template or weird things happen
            if (child == ItemTemplate.transform) { continue; }
            Destroy(child.gameObject);
        }

        // display items   
        foreach (InventoryItem item in inventory.ItemList)
        {
            GameObject goItem = Instantiate(ItemTemplate, ItemContainer);
            SetInventoryPrefab(goItem, item);
        }
    }

    protected virtual void SetInventoryPrefab(GameObject goItem, InventoryItem item)
    {
        SetInventoryPrefab_Static(goItem, item);
    }

    public static void SetInventoryPrefab_Static(GameObject gameObject, InventoryItem item)
    {
        // MetaData
        var metaData = gameObject.GetComponent<InventoryItemUIMetaData>();
        metaData.SetFromInventoryItem(item);

        // find sprit
        Image image = gameObject.GetComponentInChildren<Image>();
        image.sprite = item.Sprite;
        image.enabled = true;

        var nameText = Utils.FindInChildren(gameObject, "Text Name").GetComponent<TextMeshProUGUI>();
        nameText.text = item.Name;

        // find amount text and set
        var itemAmountText = GetAmountText(gameObject);
        if (item.Stackable)
        {
            itemAmountText.text = item.amount.ToString();
            itemAmountText.enabled = true;
        } else {
            itemAmountText.enabled = false;
        }

        // for tooltips
        ToolTipUIHelper helper = gameObject.GetComponent<ToolTipUIHelper>();
        helper.text = item.Description;

        gameObject.SetActive(true);
    }

    public static TextMeshProUGUI GetAmountText(GameObject goItem)
    {
        return Utils.FindInChildren(goItem, "Text Quantity").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Finds the transform for the first inventory item of the given type, or null
    /// </summary>
    private Transform FindInventoryItemTransform(ItemType type)
    {
        foreach (Transform child in ItemContainer)
        {
            // skip the template
            if (child == ItemTemplate.transform) { continue; }
            var metaData = child.GetComponent<InventoryItemUIMetaData>();
            if (metaData.type == type)
            {
                return child;
            }
        }
        return null;
    }

    private void Inventory_OnItemListChanged(object sender, InventoryChangedEventArgs e)
    {
        // find item that changed and add popUp
        var child = FindInventoryItemTransform(e.Type);
        if (child is Transform)
        {
            PopUpTextDriverV1.CreateInventoryChangePopUp(child, e.Delta, e.Type);
        }

        UpdateInventoryUI();
    }
}
