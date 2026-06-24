using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyBattle : MonoBehaviour, BattleActionInterface
{
    [SerializeField] private StatsSO enemyStats;
    [SerializeField] private int test;

    private Animator _animator;
    private PlayerBattleManager playerObject;
    private bool isMoving = false;
    private bool doneAttacking = false;
    private Rigidbody2D rb;
    private BattleUIManager battleManager;
    private Vector3 posisiAwal;
    private float speed = 15;


    private void Start()
    {
        enemyStats = ScriptableObject.Instantiate(enemyStats);
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        battleManager = FindAnyObjectByType<BattleUIManager>();
        posisiAwal = transform.position;
    }

    private void Update()
    {
        if(battleManager.GetCurrentTurn() == EnumTurns.enemy)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (isMoving)
        {
            _animator.SetBool("isMoving", true);
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

    public void Attack()
    {
        if (!isMoving && battleManager.GetCurrentTurn() == EnumTurns.enemy)
        {
            _animator.SetTrigger("isAttack");
            StartCoroutine(DoneAttacking(2f));
        }
    }

    public void Special()
    {
        //special here
    }

    public void Ultimate()
    {
        //ultimate here
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
            _animator.SetBool("isDeath", true);
            //kembali ke screen open world
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
            _animator.SetBool("isMoving", false);
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
