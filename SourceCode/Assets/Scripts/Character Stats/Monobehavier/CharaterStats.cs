using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterStats : MonoBehaviour
{
    public event Action<int, int> updateHealthBarOnAttack;

    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;
    #region Poverty read from data_so
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }

    public int BaseDeffence
    {
        get { if (characterData != null) return characterData.baseDeffence; else return 0; }
        set { characterData.baseDeffence = value; }
    }

    public int CurrentDeffence
    {
        get { if (characterData != null) return characterData.currentDeffence; else return 0; }
        set { characterData.currentDeffence = value; }
    }
    #endregion

    #region Character Combat

    public void takeDamage(CharaterStats attacker, CharaterStats defencer)
    {
        int damage = Mathf.Max(attacker.currentDamage() - defencer.CurrentDeffence,0);
        CurrentHealth = Math.Max(CurrentHealth - damage, 0);

        if(attacker.isCritical)
        {
            defencer.GetComponent<Animator>().SetTrigger("Hit");
        }
        //TODO:Update UI
        updateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //TODO:experience Update
        if (CurrentHealth <= 0)
            attacker.characterData.UpdateExp(characterData.killingPoint);
    }
    public void takeDamage(int damage, CharaterStats defencer)
    {
        int currentDamage = Mathf.Max(damage - defencer.CurrentDeffence, 0);
        defencer.CurrentHealth = Math.Max(defencer.CurrentHealth - currentDamage, 0);
        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killingPoint);
    }

    private int currentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical)
            coreDamage *= attackData.criticalMultiplier;

        return (int)coreDamage;
    }
    #endregion

    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
    }
}
