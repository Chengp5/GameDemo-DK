using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack Data",menuName ="Combat/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("范围")]
    public float attackRange;
    public float skillRange;

    [Header("冷却")]
    public float coolDown;

    [Header("伤害")]
    public int minDamage;
    public int maxDamage;

    [Header("暴击")]
    public float criticalMultiplier;
    public float criticalChance;

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;
        minDamage=weapon.minDamage;
        maxDamage=weapon.maxDamage;

        criticalMultiplier=weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance; ;

}
}
