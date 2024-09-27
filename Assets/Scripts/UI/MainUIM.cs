using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIM : MonoBehaviour
{
    public static MainUIM instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one Main UI Manager in scene");
        }
        instance = this;
    }

    [Header("Setup")]
    [SerializeField] private BaseStatesUIM _baseStates;
    [SerializeField] private WeaponsUIM _weapons;
    [SerializeField] private InventoryUIM _inventory;
    [SerializeField] private BottleUIM _bottle;

    public BaseStatesUIM baseStates { get { return _baseStates; } }
    public WeaponsUIM weapons { get {  return _weapons; } }
    public InventoryUIM inventory { get { return _inventory; } }
    public BottleUIM bottle { get { return _bottle; } }
}
