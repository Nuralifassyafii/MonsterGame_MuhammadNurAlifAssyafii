using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsSO", menuName = "Scriptable Objects/StatsSO")]
public class StatsSO : ScriptableObject
{
    public int hp;
    public int maxHp;
    public int mana;
    public int maxMana;
    public int attackPower;
    public int energy;
    public int maxEnergy;
    public int healthItem;
}
