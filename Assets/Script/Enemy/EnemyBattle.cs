using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyBattle : MonoBehaviour, BattleActionInterface
{
    [SerializeField] private StatsSO enemyStats;
    [SerializeField] private GameObject enemySprites;
    
    private string IS_MOVING = "isMoving";
    private string IS_ATTACK = "isAttack";
    private string IS_SPECIAL = "isSpecial";
    private string IS_ULTIMATE = "isUltimate";
    private Animator _animator;
    private PlayerBattleManager playerObject;
    private bool isMoving = false;
    private bool doneAttacking = false;
    private Rigidbody2D rb;
    private BattleUIManager battleManager;
    private Vector3 posisiAwal;
    private float speed = 15;
    private int skillCost = 5;


    private void Start()
    {
        enemyStats = ScriptableObject.Instantiate(enemyStats);
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        battleManager = FindAnyObjectByType<BattleUIManager>();
        posisiAwal = transform.position;
    }

    public void SetEnemySprites(bool isActive)
    {
        enemySprites.SetActive(isActive);
    }

    private void Update()
    {
        if(battleManager.GetCurrentTurn() == EnumTurns.enemy)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (isMoving)
        {
            _animator.SetBool(IS_MOVING, true);
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
        if (doneAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, posisiAwal, speed * Time.deltaTime);
        }
    }

    public IEnumerator DelayTurn(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        TriggerMovement();
    }

    private IEnumerator DoneAttacking(float seconds)
    {
        Cursor.lockState = CursorLockMode.Locked;
        battleManager.SetCurrentTurn(EnumTurns.standby);
        yield return new WaitForSeconds(seconds);
        doneAttacking = true;
        EndTurn();
        Cursor.lockState = CursorLockMode.None;
    }

    public void EndTurn()
    {
        battleManager.SetCurrentTurn(EnumTurns.player);
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void AddMana(int amount)
    {
        enemyStats.mana += amount;
        if(enemyStats.mana > enemyStats.maxMana)
        {
            enemyStats.mana = enemyStats.maxMana;
        }
    }

    public void AddEnergy(int amount)
    {
        enemyStats.energy += amount;
        if(enemyStats.energy > enemyStats.maxEnergy)
        {
            enemyStats.energy = enemyStats.maxEnergy;
        }
    }

    public void DecreaseMana(int amount)
    {
        enemyStats.mana -= amount;
        if (enemyStats.mana < enemyStats.maxMana)
        {
            enemyStats.mana = 0;
        }
    }

    public void DecreaseEnergy()
    {
        enemyStats.energy = 0;
    }

    public void Attack()
    {
        if (!isMoving && battleManager.GetCurrentTurn() == EnumTurns.enemy)
        {
            _animator.SetTrigger(IS_ATTACK);
            StartCoroutine(DoneAttacking(2f));
        }
    }

    public void Special()
    {
        if (!isMoving && battleManager.GetCurrentTurn() == EnumTurns.enemy)
        {
            _animator.SetTrigger(IS_SPECIAL);
            StartCoroutine(DoneAttacking(2f));
        }
    }

    public void Ultimate()
    {
        if (!isMoving && battleManager.GetCurrentTurn() == EnumTurns.enemy && enemyStats.energy != 0)
        {
            _animator.SetTrigger(IS_ULTIMATE);
            StartCoroutine(DoneAttacking(2f));
        }
    }

    public void DecreaseEnemyHealth(int attackPower)
    {
        try
        {
            int modifiedHealth = playerObject.GetPlayerStats().hp - attackPower;
            playerObject.SetPlayerHealth(modifiedHealth);
            playerObject.PlayIsHurt();
        }
        catch (Exception e)
        {
            Debug.LogError("Terjadi kesalahan saat mengurangi HP musuh : " + e.Message); //nanti diganti pakai Notif (masih belum)
        }
    }

    public StatsSO GetEnemyStats()
    {
        return enemyStats;
    }

    public void TriggerMovement()
    {
        doneAttacking = false;
        isMoving = true;
        _animator.SetBool("isMoving", true);
    }

    public void PlayIsHurt()
    {
        if(enemyStats.hp > 0)
        {
            _animator.SetTrigger("isHurt");
        }
        else
        {
            battleManager.SetCurrentTurn(EnumTurns.player);
            _animator.SetBool("isDeath", true);
            battleManager.FinishBattle();
        }
    }

    public void SetAttack()
    {
        if(enemyStats.energy == enemyStats.maxEnergy)
        {
            Ultimate();
        }
        else if(enemyStats.mana > skillCost)
        {
            Special();
        }
        else
        {
            Attack();
        }
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void SetEnemyHealth(int health)
    {
        enemyStats.hp = health;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            isMoving = false;
            _animator.SetBool(IS_MOVING, false);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (collision.gameObject.tag.Equals("Player"))
            {
                playerObject = collision.gameObject.GetComponent<PlayerBattleManager>();
            }

            Attack();
        }
        catch (Exception e)
        {
            Debug.LogError("ada yang salah saat memasukan object player : " + e.Message);
        }
    }
}
