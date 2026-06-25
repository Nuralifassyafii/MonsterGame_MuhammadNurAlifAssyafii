using System.Collections;
using UnityEngine;

public interface BattleActionInterface
{
    public void Attack();
    public void Special();
    public void Ultimate();
    public void Heal();
    public void TriggerMovement();
    public void EndTurn();
    public void DecreaseEnemyHealth(int attackPower);
    public void AddMana(int amount);
    public void AddEnergy(int amount);
    public void DecreaseMana(int amount);
    public void DecreaseEnergy();
}
