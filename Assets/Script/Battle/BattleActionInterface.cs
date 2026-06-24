using System.Collections;
using UnityEngine;

public interface BattleActionInterface
{
    public void Attack();
    public void Special();
    public void Ultimate();
    public void TriggerMovement();
    public void EndTurn();
    public void DecreaseEnemyHealth(int attackPower);
}
