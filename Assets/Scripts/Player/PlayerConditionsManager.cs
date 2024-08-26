using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerConditionsManager : MonoBehaviour
{
    private static PlayerConditionsManager _instance;
    public static PlayerConditionsManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<PlayerConditionsManager>();
                _instance.name = _instance.GetType().ToString();
            }
            return _instance;
        }
    }

    private PlayerConditions _currentCondition;
    public PlayerConditions currentCondition
    {
        get { return _currentCondition; }
        set
        {
            // Смерть - необратимое состояние, которое сбрасывается только перезапуском сцены
            if (_currentCondition == PlayerConditions.Death)
                return;

            SetConditionParameters(value);
            _currentCondition = value;
        }
    }

    bool onBattle;

    // Gaming Conditions (Игровые состояния) - состояния, в которых
    // игрок непосредственно играет в игру (бегает по уровню, стреляет и т.п.) 
    // Пример: Default, Battle
    public void SetGamingCondition()
    {
        if (onBattle)
            currentCondition = PlayerConditions.Battle;
        else
            currentCondition = PlayerConditions.Default;
    }

    void SetConditionParameters(PlayerConditions value)
    {
        switch (value)
        {
            case PlayerConditions.Default:
                onBattle = false;
                Time.timeScale = 1f;
                AudioManager.instance.normalSnapshot.TransitionTo(0.5f);
                break;
            case PlayerConditions.Battle:
                onBattle = true;
                Time.timeScale = 1f;
                AudioManager.instance.battleSnapshot.TransitionTo(2f);
                break;
            case PlayerConditions.Pause:
                Time.timeScale = 0f;
                AudioManager.instance.inMenuSnapshot.TransitionTo(0.5f);
                break;
            case PlayerConditions.Shoping:
                Time.timeScale = 0f;
                AudioManager.instance.inMenuSnapshot.TransitionTo(0.5f); // снэпшот как у паузы
                break;
            case PlayerConditions.Death:
                onBattle = false;
                Time.timeScale = 0.1f;
                AudioManager.instance.onDeathSnapshot.TransitionTo(0.1f);
                break;
        }
    }
}
