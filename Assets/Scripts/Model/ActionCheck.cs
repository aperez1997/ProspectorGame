
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
}

/// <summary>
/// Like actionCheck, but used when looking at items
/// </summary>
public class ActionCheckItem : ActionCheck
{
    /// <summary>
    /// true if applicable to item. This is used to show/hide item action buttons
    /// </summary>
    public bool IsApplicableToItem;


    public ActionCheckItem(bool canDo, SumDescription cost, bool isApplicableToItem, string reason)
        : base(canDo, cost, reason)
    {
        IsApplicableToItem = isApplicableToItem;
    }

    // method 1: create with cost and adjust
    public ActionCheckItem(SumDescription cost)
        : base(true, cost, string.Empty)
    {
        IsApplicableToItem = true;
    }

    public ActionCheckItem NotApplicableToItem()
    {
        IsAble = false;
        IsApplicableToItem = false;
        Reason = "Does not apply to this item";
        return this;
    }

    public ActionCheckItem CantForReason(string reason)
    {
        IsAble = false;
        Reason = reason;
        return this;
    }

    public ActionCheckItem NotEnoughAP()
    {
        IsAble = false;
        Reason = "Not enough AP";
        return this;
    }

    // method 2: create the one you want with statics
    public new static ActionCheckItem IsAbleTo(SumDescription cost)
    {
        return new ActionCheckItem(cost);
    }

    public static ActionCheckItem NotApplicableToItem(SumDescription cost)
    {
        var check = new ActionCheckItem(cost);
        return check.NotApplicableToItem();
    }

    public new static ActionCheckItem CantForReason(SumDescription cost, string reason)
    {
        var check = new ActionCheckItem(cost);
        return check.CantForReason(reason);
    }

    public new static ActionCheckItem NotEnoughAP(SumDescription cost)
    {
        var check = new ActionCheckItem(cost);
        return check.NotEnoughAP();
    }
}