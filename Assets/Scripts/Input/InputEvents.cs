using System;
using UnityEngine;

public class InputEvents
{
    public event Action onAttackPressed;
    public void AttackPressed()
    {
        if (onAttackPressed != null)
            onAttackPressed();
    }

    public event Action onDashPressed;

    public void DashPressed()
    {
        if (onDashPressed != null)
            onDashPressed();
    }

    public event Action onRunPressed;

    public void RunPressed() => onRunPressed?.Invoke();

    public event Action onRunCanceled;

    public void RunCanceled() => onRunCanceled?.Invoke();

    public event Action onInteractPressed;

    public void InteractPressed()
    {
        if (onInteractPressed != null)
            onInteractPressed();
    }

    public event Action onReloadPressed;
    public void ReloadPressed()
    {
        if (onReloadPressed != null)
            onReloadPressed();
    }

    public event Action onChangeWeaponPressed;
    public void ChangeWeaponPressed()
    {
        if (onChangeWeaponPressed != null)
            onChangeWeaponPressed();
    }

    public event Action onRemoveWeaponPressed;
    public void RemoveWeaponPressed()
    {
        if (onRemoveWeaponPressed!= null)
            onRemoveWeaponPressed();
    }

    public event Action<InputHandlers> onChangeHandling;

    public void ChangeHandling(InputHandlers handler)
    {
        if (onChangeHandling != null)
            onChangeHandling(handler);
    }

    public event Action<Vector2> onMoveDirectionChanged;

    public void MoveDirectionChanged(Vector2 moveDirection)
    {
        if (onMoveDirectionChanged != null)
            onMoveDirectionChanged(moveDirection);
    }

    public event Action<Vector2> onLookDirectionChanged;

    public void LookDirectionChanged(Vector2 lookDirection)
    {
        if (onLookDirectionChanged != null)
            onLookDirectionChanged(lookDirection);
    }

    public event Action onInventoryPerformed;

    public void InventoryPerformed()
    {
        if (onInventoryPerformed != null)
            onInventoryPerformed();
    }

    public event Action onInventoryCanceled;

    public void InventoryCanceled()
    {
        if (onInventoryCanceled != null)
            onInventoryCanceled();
    }

    public event Action onPausePressed;

    public void PausePressed()
    {
        if (onPausePressed != null)
            onPausePressed();
    }

    public event Action onFirstWeaponChoosen;

    public void FirstWeaponChoosen() 
    {
        if (onFirstWeaponChoosen != null)
            onFirstWeaponChoosen();
    }

    public event Action onSecondWeaponChoosen;

    public void SecondWeaponChoosen()
    {
        if (onSecondWeaponChoosen != null)
            onSecondWeaponChoosen();
    }

    public event Action onThirdWeaponChoosen;

    public void ThirdWeaponChoosen()
    {
        if (onThirdWeaponChoosen != null)
            onThirdWeaponChoosen();
    }

    public event Action onMeleeWeaponChoosen;

    public void MeleeWeaponChoosen()
    {
        if (onMeleeWeaponChoosen != null)
            onMeleeWeaponChoosen();
    }
    public event Action onGrenadeAttack;

    public void GrenadeAttack() 
    {
        if (onGrenadeAttack!=null) {
            onGrenadeAttack();
        }
    }
    public event Action onHealthBottle;

    public void HealthBottle() {
        if (onHealthBottle != null)
        {
            onHealthBottle();
        }
    }

    public event Action onMapPressed;
    public void MapPressed()
    {
        if (onMapPressed != null)
            onMapPressed();
    }

    public event Action onMapReleased;
    public void MapReleased() => onMapReleased?.Invoke();
}
