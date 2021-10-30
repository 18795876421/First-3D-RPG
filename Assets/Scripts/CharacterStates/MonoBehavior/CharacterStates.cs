using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public CharacterData_SO templateData;  //模板数据
    public CharacterData_SO characterData;  //人物数据
    public AttackData_SO attackData;  //攻击数据
    public bool isCritical;  //暴击

    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);  //复制一个 templateData 给 characterData
    }

    #region Read from Data_SO

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

    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }

    public int CurretnDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }

    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStates attacker, CharacterStates defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurretnDefence, 1);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //TODO:更新UI
        //判断死亡
        //TODO:经验值
    }

    private int CurrentDamage()
    {
        int coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical)
        {
            coreDamage *= (int)attackData.criticalMultiplier;
        }
        return coreDamage;
    }

    #endregion 
}