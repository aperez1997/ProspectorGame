using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private InventoryController UIInventory;

    private Inventory inventory;


    private void Awake()
    {
        Instance = this;

        inventory = new Inventory();
        UIInventory.SetInventory(inventory);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
