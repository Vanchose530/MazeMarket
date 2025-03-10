using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerConditionsEvents
{
    public event UnityAction onBattleStarts;
    public void BattleStarts() => onBattleStarts?.Invoke();
}
