using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public CharacterData_SO characterData;  //人物数据
    public AttackData_SO attackData;  //攻击数据
    [HideInInspector]
    public bool isCritical;  //暴击
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
}
