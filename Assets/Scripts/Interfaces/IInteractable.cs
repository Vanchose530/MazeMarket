using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(Player player);

    public void CanInteract(Player player);

    public void CanNotInteract(Player player);
}
