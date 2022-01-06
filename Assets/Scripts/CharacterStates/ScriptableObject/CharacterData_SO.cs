using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newData", menuName = "Character State/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("State Info")]
    public int maxHealth;  //最大血量
    public int currentHealth; //当前血量
    public int baseDefence; //基础防御
    public int currentDefence; //当前防御

    [Header("Dead")]
    public float provideExp; //死亡时提供的经验

    [Header("Experience")]
    public float currentExp; //当前经验
    public float levelUpExp; //升级所需经验

    [Header("Level")]
    public int currentLevel; //当前等级
    public int maxLevel; //最大等级
    public float levelCoefficient; //升级系数

    public void UpdateExp(float provideExp)
    {
        currentExp += provideExp;
        if (currentExp >= levelUpExp)
        {
            currentExp -= levelUpExp;
            LevelUp();
        }
    }

    //升级，更新属性
    private void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 1, maxLevel); //等级 +1
        levelUpExp += levelUpExp * (currentLevel - 1) * levelCoefficient; //升级所需经验增加 10%
        maxHealth += (int)(maxHealth * levelCoefficient); //最大血量增加 10%
        baseDefence += 1; //基础防御 +1
        currentHealth = maxHealth; //恢复满血
    }
}