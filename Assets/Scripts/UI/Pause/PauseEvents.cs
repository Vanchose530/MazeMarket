using System;

public class PauseEvents
{
    public event Action onPauseIn;

    public event Action onPauseOut;

    public void PauseIn()
    {
        if (onPauseIn != null)
            onPauseIn();
    }

    public void PauseOut()
    {
        if (onPauseOut != null)
            onPauseOut();
    }
}
