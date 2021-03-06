using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple class used to communicate whether an action can be done or not,
/// along with some helpful information about the circumstances
/// </summary>
public class ActionCheck
{
    /// <summary>
    /// If the πlayer is able to or not
    /// </summary>
    public bool IsAble;
    /// <summary>
    /// Description of cost
    /// </summary>
    public SumDescription Cost;
    /// <summary>
    /// Reason to explain, used when not able
    /// </summary>
    public string Reason;
    /// <summary>
    /// Optional chance, if that is something that matters
    /// </summary>
    public int Chance;

    public ActionCheck(bool able, SumDescription cost, string reason)
    {
        IsAble = able;
        Cost = cost;
        Reason = reason;
    }

    public ActionCheck(SumDescription cost) : this(true, cost, string.Empty)
    { }

    public ActionCheck NotEnoughAP()
    {
        IsAble = false;
        Reason = "Not enough AP";
        return this;
    }

    public static ActionCheck IsAbleTo(SumDescription cost)
    {
        return new ActionCheck(true, cost, string.Empty);
    }

    public static ActionCheck CantForReason(SumDescription cost, string reason)
    {
        return new ActionCheck(false, cost, reason);
    }

    public static ActionCheck NotEnoughAP(SumDescription cost)
    {
        return new ActionCheck(false, cost, "Not enough AP");
    }

    public override string ToString()
    {
        return "Check[" + IsAble.ToString() + "," + Cost.ToString() +","+ Reason + "]";
    }
}

/// <summary>
/// Sub class for ItemActions
/// </summary>
public class ItemActionCheck : ActionCheck
{
    /// <summary>
    /// true if applicable to item. This is used to show/hide item action buttons
    /// </summary>
    public bool IsApplicableToItem;

    public ItemActionCheck(bool canDo, SumDescription cost, bool isApplicableToItem, string reason)
        : base(canDo, cost, reason)
    {
        IsApplicableToItem = isApplicableToItem;
    }

    // method 1: create with cost and adjust
    public ItemActionCheck(SumDescription cost)
        : base(cost)
    {
        IsApplicableToItem = true;
    }

    public ItemActionCheck NotApplicableToItem()
    {
        IsAble = false;
        IsApplicableToItem = false;
        Reason = "Does not apply to this item";
        return this;
    }

    public ItemActionCheck CantForReason(string reason)
    {
        IsAble = false;
        Reason = reason;
        return this;
    }

    public new ItemActionCheck NotEnoughAP()
    {
        return (ItemActionCheck) base.NotEnoughAP();
    }

    // method 2: create the one you want with statics
    public new static ItemActionCheck IsAbleTo(SumDescription cost)
    {
        return new ItemActionCheck(cost);
    }

    public static ItemActionCheck NotApplicableToItem(SumDescription cost)
    {
        var check = new ItemActionCheck(cost);
        return check.NotApplicableToItem();
    }

    public new static ItemActionCheck CantForReason(SumDescription cost, string reason)
    {
        var check = new ItemActionCheck(cost);
        return check.CantForReason(reason);
    }

    public new static ItemActionCheck NotEnoughAP(SumDescription cost)
    {
        var check = new ItemActionCheck(cost);
        return check.NotEnoughAP();
    }
}

/// <summary>
/// Represents an action button in the UI
/// </summary>
public class ActionButton
{
    /// <summary>
    /// The name of the action. Shown to user
    /// </summary>
    public string Name;

    /// <summary>
    /// Structure that determines if the action is useable and any reasons behind that
    /// </summary>
    public ActionCheck Check;

    /// <summary>
    /// Code the gets executed when the button is clicked
    /// </summary>
    public Action<GameObject> Action;

    public ActionButton(string name, ActionCheck check, Action<GameObject> action)
    {
        Name = name;
        Check = check;
        Action = action;
    }

    public override string ToString()
    {
        var checkStr = Check is ActionCheck ? Check.ToString() : "null";
        return "AB[" + Name + "," + checkStr + "]";
    }
}

/// <summary>
/// Sub class for Item Actions Buttons (buttons that perform actions on items)
/// </summary>
public class ItemActionButton : ActionButton
{
    /// <summary>
    /// Different class for the check
    /// </summary>
    public new ItemActionCheck Check;

    public ItemActionButton(string name, ItemActionCheck check, Action<GameObject> action)
        : base(name, check, action)
    {
        Check = check;
    }
}


/// <summary>
/// Represents the result of an action being performed
/// </summary>
public class ActionResult
{
    public bool IsSuccess;
    public ActionCheck Check;

    // TODO: need some kind of result structure (items that were gained, costs that were paid?)

    public bool IsAble { get { return Check.IsAble; } }
    public string Reason { get { return Check.Reason; } }

    public ActionResult(bool isSuccess, ActionCheck check)
    {
        IsSuccess = isSuccess;
        Check = check;
    }

    public ActionResult(ActionCheck check) : this(check.IsAble, check)
    { }

    public ActionResult NotEnoughAP()
    {
        this.Check.NotEnoughAP();
        IsSuccess = false;
        return this;
    }
}

/// <summary>
/// sub class of Action Results pertaining to Actions on items
/// </summary>
public class ItemActionResult : ActionResult
{
    public new ItemActionCheck Check;

    public ItemActionResult(bool isSuccess, ItemActionCheck check)
        : base(isSuccess, check)
    {
        Check = check;
    }

    public ItemActionResult(ItemActionCheck check) : this(check.IsAble, check)
    {}

    public new ItemActionResult NotEnoughAP()
    {
        return (ItemActionResult) base.NotEnoughAP();
    }
}