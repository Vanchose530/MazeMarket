using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{

    // Player
    public int maxHealth;
    public float maxStamina;
    public int heath;
    public Vector2 nextLevelStartPosition;

    // Weapons
    public int lightBullets;
    public int mediumBullets;
    public int heavyBullets;
    public int shells;

    public Gun gun1;
    public Gun gun2;
    public Gun gun3;
    public MeleeWeapon meleeWeapon;

    // Inventory
    public int moneyCount;
    public int voidBottleCount;
    public int demonsBloodGrenadeCount;
    public int healthPoitionCount;

}
