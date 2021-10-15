using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newData", menuName = "Character State/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("State Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;
}