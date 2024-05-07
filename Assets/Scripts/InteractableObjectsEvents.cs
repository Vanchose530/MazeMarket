using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectsEvents
{
    public event Action onCanInteractObject;
    public void CanInteractObject() => onCanInteractObject?.Invoke();

    public event Action onCanNotInteractObject;
    public void CanNotInteractObject() => onCanNotInteractObject?.Invoke();
}
