using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : ScriptableObject
{
    [HideInInspector] public Action onAttack;

    [Header("Wepon settings")]
    public string displayName;
    public int damage;
    public Sprite image;
    public int id;
    public bool holdToAttack;

    [Header("Weapon sound effects")]
    [SerializeField] private SoundEffect _pickUpSE;
    public SoundEffect pickUpSE { get { return _pickUpSE; } }

    /*private void OnValidate()
    {
        #if UNITY_EDITOR
        displayName = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }*/

    abstract public void Attack(Transform attackPointPosition);
}
