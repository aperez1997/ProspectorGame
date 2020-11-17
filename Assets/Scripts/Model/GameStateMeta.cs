using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains misc meta information about the game itself
/// </summary>
[Serializable]
public class GameStateMeta
{
    /// <summary>
    /// identifies save
    /// </summary>
    public Guid Guid;

    /// <summary>
    /// In game date
    /// </summary>
    [SerializeField] private DateTime _gameDate;
    public DateTime GameDate {
        get { return _gameDate; }
        set {
            _gameDate = value;
            OnGameDateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler<EventArgs> OnGameDateChanged;

    public GameStateMeta()
    {
        this.Guid = System.Guid.NewGuid();
        this._gameDate = new DateTime(1849, 5, 1);
    }

    public void AddDays(int days = 1)
    {
        GameDate = GameDate.AddDays(days);
    }
}
