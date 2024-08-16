using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<GameObject>(PATH_TO_SINGLETON_PREFAB);
                var inScene = Instantiate<GameObject>(prefab);
                _instance = inScene.GetComponentInChildren<InputManager>();

                if (_instance == null )
                    _instance = inScene.AddComponent<InputManager>();

                DontDestroyOnLoad(_instance.transform.root.gameObject);
            }

            return _instance;
        }
    }

    const string PATH_TO_SINGLETON_PREFAB = "Singletons\\Input Manager";
    public InputHandlers inputHandler { get; private set; }

    public Vector2 moveDirection { get; private set; }
    public Vector2 lookDirection { get; private set; }
    public Vector2 mousePosition { get; private set; }

    private bool attackPressed;
    private bool dashPressed;
    private bool runPressed;
    private bool interactPressed;
    private bool reloadPressed;
    private bool inventoryPressed;
    private bool changeWeaponPressed;
    private bool removeWeaponPressed;
    private bool pausePressed;
    private bool grenadeAttack;
    private bool healthBottle;

    private bool chooseWeapon;
    private bool chooseWeapon2;
    private bool chooseWeapon3;
    private bool chooseWeapon4;

    private void Awake()
    {
        Invoke("SetStartHandling", 0.1f);
    }

    private void Update()
    {
        if (inputHandler == InputHandlers.KeyboardAndMouse)
        {
            mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        else if (inputHandler == InputHandlers.Gamepad)
        {
            if (moveDirection == Vector2.zero)
            {
                runPressed = false; // для бега на геймпаде
            }
        }
    }

    private void SetStartHandling()
    {
        ChangeHandling(inputHandler);

        if (inputHandler == InputHandlers.KeyboardAndMouse)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void ChangeHandling(InputHandlers handler)
    {
        if (inputHandler == handler) return;

        inputHandler = handler;

        if (inputHandler == InputHandlers.KeyboardAndMouse)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;

        GameEventsManager.instance.input.ChangeHandling(handler);
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
            GameEventsManager.instance.input.MoveDirectionChanged(moveDirection);
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
            GameEventsManager.instance.input.MoveDirectionChanged(moveDirection);
        }
    }

    public void LookPressed(InputAction.CallbackContext context)
    {
        ChangeHandling(InputHandlers.Gamepad);

        if (context.performed)
        {
            lookDirection = context.ReadValue<Vector2>();
        }
    }

    public void MouseMoved(InputAction.CallbackContext context) // сделать норм поворот камеры с мыши
    {
        ChangeHandling(InputHandlers.KeyboardAndMouse);

        if (context.performed)
        {
            mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        }

        if (inputHandler == InputHandlers.KeyboardAndMouse)
        {
            lookDirection = (mousePosition - (Vector2)Player.instance.rb.position).normalized;
        }
    }

    public void AttackPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackPressed = true;
            GameEventsManager.instance.input.AttackPressed();
        }
        else if (context.canceled)
        {
            attackPressed = false;
        }
    }

    public void DashPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dashPressed = true;
            GameEventsManager.instance.input.DashPressed();
        }
        else if (context.canceled)
        {
            dashPressed = false;
        }
    }

    public void RunPressed(InputAction.CallbackContext context)
    {
        if (inputHandler == InputHandlers.KeyboardAndMouse)
        {
            if (context.performed)
            {
                runPressed = true;
                GameEventsManager.instance.input.RunPressed();
            }
            else if (context.canceled)
            {
                runPressed = false;
            }
        }
        else if (inputHandler == InputHandlers.Gamepad)
        {
            if (context.performed)
            {
                runPressed = true;
                GameEventsManager.instance.input.RunPressed();
            }
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
            GameEventsManager.instance.input.InteractPressed();
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void ReloadPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            reloadPressed = true;
            GameEventsManager.instance.input.ReloadPressed();
        }
        else if (context.canceled)
        {
            reloadPressed = false;
        }
    }

    public void InventoryPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventoryPressed = true;
            GameEventsManager.instance.input.InventoryPerformed();
        }
        else if (context.canceled)
        {
            inventoryPressed = false;
            GameEventsManager.instance.input.InventoryCanceled();
        }
    }

    public void ChangeWeaponPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            removeWeaponPressed = true;
            GameEventsManager.instance.input.ChangeWeaponPressed();
        }
        else if (context.canceled)
        {
            removeWeaponPressed = false;
        }
    }

    public void RemoveWeaponPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            removeWeaponPressed = true;
            GameEventsManager.instance.input.RemoveWeaponPressed();
        }
        else if (context.canceled)
        {
            removeWeaponPressed = false;
        }
    }

    public void PausePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pausePressed = true;
            GameEventsManager.instance.input.PausePressed();
        }
        else if (context.canceled)
        {
            pausePressed = false;
        }
    }

    public void MapPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameEventsManager.instance.input.MapPressed();
        }
    }

    public bool GetAttackPressed(bool hold = false)
    {
        if (hold)
        {
            return attackPressed;
        }
        else
        {
            bool result = attackPressed;
            attackPressed = false;
            return result;
        }
    }

    public bool GetDashPressed(bool hold = false)
    {
        if (hold)
        {
            return dashPressed;
        }
        else
        {
            bool result = dashPressed;
            dashPressed = false;
            return result;
        }
    }

    public bool GetRunPressed(bool hold = false)
    {
        if (hold)
        {
            return runPressed;
        }
        else
        {
            bool result = runPressed;
            runPressed = false;
            return result;
        }
    }

    public bool GetInteractPressed(bool hold = false)
    {
        if (hold)
        {
            return interactPressed;
        }
        else
        {
            bool result = interactPressed;
            interactPressed = false;
            return result;
        }
    }

    public bool GetReloadPressed()
    {
        bool result = reloadPressed;
        reloadPressed = false;
        return result;
    }

    public void FirstWeaponChoosen(InputAction.CallbackContext context)
    {
        Debug.Log("1");
        if (context.performed)
        {
            chooseWeapon = true;
            GameEventsManager.instance.input.FirstWeaponChoosen();
        }
        else if (context.canceled)
        {
            chooseWeapon = false;
        }
    }

    public void SecondWeaponChoosen(InputAction.CallbackContext context)
    {
        Debug.Log("2");
        if (context.performed)
        {
            chooseWeapon2 = true;
            GameEventsManager.instance.input.SecondWeaponChoosen();
        }
        else if (context.canceled)
        {
            chooseWeapon2 = false;
        }
    }

    public void ThirdWeaponChoosen(InputAction.CallbackContext context)
    {
        Debug.Log("3");
        if (context.performed)
        {
            chooseWeapon3 = true;
            GameEventsManager.instance.input.ThirdWeaponChoosen();
        }
        else if (context.canceled)
        {
            chooseWeapon3 = false;
        }
    }

    public void MeleeWeaponChoosen(InputAction.CallbackContext context)
    {
        Debug.Log("4");
        if (context.performed)
        {
            chooseWeapon4 = true;
            GameEventsManager.instance.input.MeleeWeaponChoosen();
        }
        else if (context.canceled)
        {
            chooseWeapon4 = false;
        }
    }
    public void GrenadeAttack(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            grenadeAttack = true;
            GameEventsManager.instance.input.GrenadeAttack();
        }
        else if (context.canceled)
        {
            grenadeAttack = false;
        }
    }
    public bool GetGrenadeAttack(bool hold = false)
    {
        if (hold)
        {
            return grenadeAttack;
        }
        else
        {
            bool result = grenadeAttack;
            grenadeAttack = false;
            return result;
        }
    }
    public void HealthBottle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            healthBottle = true;
            GameEventsManager.instance.input.HealthBottle();
        }
        else if (context.canceled)
        {
            healthBottle = false;
        }
    }
    public bool GetHealthBottle(bool hold = false)
    {
        if (hold)
        {
            return healthBottle;
        }
        else
        {
            bool result = healthBottle;
            healthBottle = false;
            return result;
        }
    }

}
