using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    [SerializeField] private StatsSO enemyStats;
    [SerializeField] private int test;
    private Animator _animator;

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

    public StatsSO GetEnemyStats()
    {
        return enemyStats;
    }

    public void PlayIsHurt()
    {
        _animator.SetTrigger("isHurt");
    }

    public void SetEnemyHealth(int health)
    {
        enemyStats.hp = health;
    }

    private void Start()
    {
        enemyStats = ScriptableObject.Instantiate(enemyStats);
        _animator = GetComponent<Animator>();
    }
}
