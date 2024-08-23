
using UnityEngine;

public class RandomLootSource : MonoBehaviour, IInteractable
{
    LootObject loot;
    Animator animator;
    SoundEffect lootSE;

    void IInteractable.CanInteract(Player player)
    {

    }

    void IInteractable.CanNotInteract(Player player)
    {

    }

    void IInteractable.Interact(Player player)
    {

    }
}
