using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected string id;
    [SerializeField] protected SoundEffect pickUpSE;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void Start()
    {
        if (id == null || id == "")
            Debug.LogError("For Item not setted unique id. Item object: " + gameObject.name);
        else
            Invoke("CheckToCollected", 0.01f);
    }

    protected void CheckToCollected()
    {
        if (ItemsManager.instance.collectedItemsId.Contains(id))
            Destroy(gameObject);
    }

    protected void SaveCollectedItem()
    {
        if (id != null && id != "")
            ItemsManager.instance.collectedItemsId.Add(id);
        else
            Debug.LogWarning("Tryed to save collected item but it don't have unique id. Item object: " + gameObject.name);
    }

    public abstract void PickUp();
}
