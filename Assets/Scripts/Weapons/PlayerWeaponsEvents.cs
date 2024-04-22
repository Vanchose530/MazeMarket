using System;

public class PlayerWeaponsEvents
{
    public event Action onWeaponChanged;

    public void WeaponChanged()
    {
        if (onWeaponChanged != null)
            onWeaponChanged();
    }
}
