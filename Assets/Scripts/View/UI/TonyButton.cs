using UnityEngine.UI;

/// <summary>
/// Sub-class of Unity button to add extra behaviors
/// </summary>
public class TonyButton : Button
{
    /// <summary>
    /// Make the button interactable but appear disabled
    /// </summary>
    public void SetInteractableButDisabled()
    {
        this.interactable = true;
        this.DoStateTransition(SelectionState.Disabled, true);
    }
}