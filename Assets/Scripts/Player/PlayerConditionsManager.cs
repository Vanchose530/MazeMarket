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
            // ������ - ����������� ���������, ������� ������������ ������ ������������ �����
            if (_currentCondition == PlayerConditions.Death)
                return;

            SetConditionParameters(value);
            _currentCondition = value;
        }
    }
    private bool _onBattle;
    public bool onBattle 
    {
        get { return _onBattle; }
        set 
        {
            if (!_onBattle && value)
                GameEventsManager.instance.playerConditions.BattleStarts();

            _onBattle = value;
        }
    }

    // Gaming Conditions (������� ���������) - ���������, � �������
    // ����� ��������������� ������ � ���� (������ �� ������, �������� � �.�.) 
    // ������: Default, Battle
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
                InputManager.instance.UnLockCursor();
                break;
            case PlayerConditions.Battle:
                onBattle = true;
                Time.timeScale = 1f;
                AudioManager.instance.battleSnapshot.TransitionTo(2f);
                InputManager.instance.UnLockCursor();
                break;
            case PlayerConditions.Pause:
                Time.timeScale = 0f;
                AudioManager.instance.inMenuSnapshot.TransitionTo(0.5f);
                InputManager.instance.LockCursor();
                break;
            case PlayerConditions.Shoping:
                Time.timeScale = 0f;
                AudioManager.instance.inMenuSnapshot.TransitionTo(0.5f); // ������� ��� � �����
                InputManager.instance.LockCursor();
                break;
            case PlayerConditions.Death:
                onBattle = false;
                Time.timeScale = 0.1f;
                AudioManager.instance.onDeathSnapshot.TransitionTo(0.1f);
                InputManager.instance.LockCursor();
                break;
        }
    }
}
