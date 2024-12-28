using System;

public class PlayerWeaponsEvents
{
    public event Action onWeaponChanged;

    public void WeaponChanged()
    {
        if (onWeaponChanged != null)
            onWeaponChanged();
    }

    public event Action onReloadingEnd;
    public void ReloadingEnd() => onReloadingEnd?.Invoke();
}
