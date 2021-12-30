using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange; //攻击范围
    public float skillRange; //技能范围
    public float attackCoolDown; //攻击冷却
    public float skillCoolDown; //技能冷却
    public int minDamage; //最小攻击伤害
    public int maxDamage; //最大攻击上海
    public float criticalMultiplier; //暴击伤害倍数
    public float criticalChance; //暴击几率
}