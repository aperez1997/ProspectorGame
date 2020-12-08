/// <summary>
/// Controller for displaying the Player's inventory
/// </summary>
public class PlayerInventoryController : InventoryController
{
    protected override void Start()
    {
        // get these from singleton. Is this the right way?
        Inventory = GameStateManager.LogicInstance.Inventory;

        base.Start();
    }
}
