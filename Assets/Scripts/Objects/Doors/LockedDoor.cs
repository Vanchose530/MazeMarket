using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door, IDataPersistence
{
    [Header("Unique ID")]
    [SerializeField] protected string id;

    [Header("Locked Door")]
    private bool _locked;
    [HideInInspector] public bool locked
    {
        get { return _locked; }
        set
        {
            ColorIndicators(value);
            _locked = value;
        }
    }
    [SerializeField] private SpriteRenderer[] indicators;
    [SerializeField] private Color lockColor;
    [SerializeField] private Color unlockColor;

    [Header("Sound Effects")]
    [SerializeField] private GameObject doorUnlockedSound;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        locked = true;

        if (id == null || id == "")
            Debug.LogError("For Locked Door not setted unique id. Locked Door object: " + gameObject.name);
    }

    public void LoadData(GameData data)
    {
        if (data.unlockedDoorsId.Contains(id))
            locked = false;
    }

    public void SaveData(ref GameData data)
    {
        if (!data.unlockedDoorsId.Contains(id) && !locked)
            data.unlockedDoorsId.Add(id);
    }

    private void ColorIndicators(bool locked)
    {
        if (locked)
        {
            foreach(var indicator in indicators)
            {
                indicator.color = lockColor;
            }
        }
        else
        {
            foreach (var indicator in indicators)
            {
                indicator.color = unlockColor;
            }
        } 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (locked && collision.CompareTag("Player") && InputManager.instance.GetInteractPressed() && PlayerInventory.instance.keyCardCount > 0)
        {
            locked = false;
            EffectsManager.instance.PlaySoundEffect(doorUnlockedSound, transform.position, 2f);
            PlayerInventory.instance.keyCardCount--;
            OnTriggerEnter2D(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !locked)
        {
            Open(openCloseSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Close(openCloseSpeed);
        }
    }
}
