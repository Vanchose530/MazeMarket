
using UnityEngine;
[CreateAssetMenu(fileName = "New Certain Loot Source", menuName = "Scriptable Objects/LootSources", order = 1)]
public class CertainLootSource : MonoBehaviour, IInteractable
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
        if (loot.GetCanLoot())
            loot.Loot();
    }
}
