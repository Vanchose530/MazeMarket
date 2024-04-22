using System;

public class PlayerEvents
{
    public event Action onPlayerDeath;

    public void PlayerDeath()
    {
        if (onPlayerDeath != null)
            onPlayerDeath();
    }
}
