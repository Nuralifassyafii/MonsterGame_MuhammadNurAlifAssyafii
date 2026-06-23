using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    [SerializeField] private StatsSO enemyStats;
    [SerializeField] private int test;

    public void Attack()
    {
        //attack here
    }

    public void Special()
    {
        //special here
    }

    public void Ultimate()
    {
        //ultimate here
    }

    private void Start()
    {
        enemyStats = ScriptableObject.Instantiate(enemyStats);
    }
}
