using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Useable,
    Weapon,
    Armor
}

[CreateAssetMenu(fileName ="New Item", menuName ="Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType ItemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmout;
    [TextArea]
    public string discription;

    public bool stackable;

    [Header("Usable Item")]
    public UsableItemData_SO itemData;


    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponAttackData;

    public AnimatorOverrideController weaponAnimator;
}
