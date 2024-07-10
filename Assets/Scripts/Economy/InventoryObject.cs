using UnityEngine;

public abstract class InventoryObject : ScriptableObject
{
    [Header("InventoryObject")]
    [SerializeField] private Sprite _image;
    [SerializeField] private string _displayName;
    [TextArea(3,2)]
    [SerializeField] private string _description;
    [SerializeField] private int _purchasePrice;

    //public int salePrice; // TODO: sale mechanism

    public Sprite image { get { return _image; } }
    public string displayName { get { return _displayName; } }
    public int purchasePrice { get { return _purchasePrice; } }
    public string description { get { return _description; } }

    public virtual bool GetCanAddInInventory()
    {
        return true;
    }
    public abstract void ToPlayerInventory();
    
    /*public void FromPlayerInventory()
    {

    }*/
}