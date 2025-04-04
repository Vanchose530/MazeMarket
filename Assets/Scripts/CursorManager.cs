using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public static CursorManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<CursorManager>();
                _instance.name = _instance.GetType().ToString();
            }

            return _instance;
        }
    }

    [Header("Gamepad")]
    public float distanceBetweenPlayerAndAim = 5;

    private GameObject _cursorPositionGO;

    private GameObject cursorPositionGO
    {
        get
        {
            if (_cursorPositionGO == null)
            {
                _cursorPositionGO = Instantiate(new GameObject());
                _cursorPositionGO.name = "Cursor";
            }
            return _cursorPositionGO;
        }
    }

    private GameObject _aim;
    private GameObject aim
    {
        get
        {
            if (_aim == null)
            {
                _aim = Instantiate(Resources.Load<GameObject>(PATH_TO_AIM_CURSOR_PREFAB), cursorPositionGO.transform);
            }
            return _aim;
        }
    }

    private const string PATH_TO_AIM_CURSOR_PREFAB = "Cursor\\Aim";

    private bool _aimVisible;
    bool aimVisible
    {
        get { return _aimVisible; }
        set
        {
            if (value)
            {
                aim.SetActive(true);
            }
            else
            {
                // aim.transform.position = new Vector2(-1000000, -100000);
                aim.SetActive(false);
            }
            _aimVisible = value;
        }
    }

    private CursorStates _cursorStates;
    public CursorStates cursorState
    {
        get { return _cursorStates; }
        set
        {
            SetCursorState(value);
            _cursorStates = value;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerWeapons.onWeaponChanged += InvokeUpdateAimPosition;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerWeapons.onWeaponChanged -= InvokeUpdateAimPosition;
    }

    private void Update()
    {
        UpdateCursorAimPosition();
    }

    void UpdateCursorAimPosition()
    {
        if (!aimVisible)
            return;

        if (InputManager.instance.inputHandler == InputHandlers.KeyboardAndMouse)
            cursorPositionGO.transform.position = InputManager.instance.mousePosition;
        else
            cursorPositionGO.transform.position = Player.instance.rb.position + InputManager.instance.lookDirection * distanceBetweenPlayerAndAim;

        cursorPositionGO.transform.rotation = Player.instance.transform.rotation;
        aim.transform.rotation = Quaternion.identity;
    }

    public void InvokeUpdateAimPosition()
    {
        Invoke("UpdateAimPosition", 0.1f);
    }
    private void UpdateAimPosition()
    {
        aim.transform.localPosition = new Vector2(Player.instance.attackPointPosition.x, 0);
    }

    void SetCursorState(CursorStates value)
    {
        switch (value)
        {
            case CursorStates.None:
                Cursor.visible = false;
                aimVisible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorStates.Aim:
                Cursor.visible = false;
                aimVisible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case CursorStates.RealCursor:
                Cursor.visible = true;
                aimVisible = false;
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }
}
