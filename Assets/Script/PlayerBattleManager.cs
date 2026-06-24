using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EffectList
{
    public string effectName;
    public GameObject objectEffect;
}
public class PlayerBattleManager : MonoBehaviour, BattleActionInterface
{
    [SerializeField] private StatsSO playerStat;
    [SerializeField] List<EffectList> effectList;

    private bool isMoving = false;
    private bool doneAttacking = false;
    private Rigidbody2D rb;
    private Vector3 posisiAwal;
    private float speed = 15;
    private BattleUIManager _battleManager;
    private EnemyBattle enemy;
    private Animator _animator;
    private EnumActions actionPlayer = EnumActions.idle;


    private void Start()
    {
        playerStat = ScriptableObject.Instantiate(playerStat);
        _battleManager = FindFirstObjectByType<BattleUIManager>();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        posisiAwal = transform.position;
    }

    private void Update()
    {
        if (_battleManager.GetCurrentTurn() == EnumTurns.player)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (isMoving)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
        if (doneAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, posisiAwal, speed * Time.deltaTime);
        }
    }

    public StatsSO GetPlayerStats()
    {
        return playerStat;
    }

    public void SetPlayerHealth(int health)
    {
        playerStat.hp = health;
    }

    public void SetActionPlayer(int action)
    {
        actionPlayer = (EnumActions)action;
    }
    private IEnumerator DoneAttacking(float seconds)
    {
        Cursor.lockState = CursorLockMode.Locked;
        _battleManager.SetCurrentTurn(EnumTurns.standby);
        yield return new WaitForSeconds(seconds);
        doneAttacking = true;
        EndTurn();
        Cursor.lockState = CursorLockMode.None;
    }

    public void TriggerMovement()
    {
        doneAttacking = false;
        isMoving = true;
        _animator.SetBool("isMoving", true);
        _animator.SetFloat("xDirection", 1);
    }

    public void PlayIsHurt()
    {
        if (playerStat.hp > 0)
        {
            _animator.SetTrigger("isHurtTurnBase");
        }
        else
        {
            _animator.SetBool("isDeathTurnBase", true);
        }
    }

    public void ShowEffect(string effectName)
    {
        effectList.Find(item => item.effectName.Equals(effectName)).objectEffect.SetActive(true);
    }

    public void HideEffect(string effectName)
    {
        effectList.Find(item => item.effectName.Equals(effectName)).objectEffect.SetActive(false);
    }

    public void Attack()
    {
        if (!isMoving)
        {
            _animator.SetTrigger("isAttackTurnBase");
            StartCoroutine(DoneAttacking(2f));
        }
    }

    public void EndTurn()
    {
        _battleManager.SetCurrentTurn(EnumTurns.enemy);
        rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(enemy.DelayTurn(2f));
    }

    public void DecreaseEnemyHealth(int attackPower)
    {
        try
        {
            int modifiedHealth = enemy.GetEnemyStats().hp - attackPower;
            enemy.SetEnemyHealth(modifiedHealth);
            enemy.PlayIsHurt();
        }
        catch (Exception e)
        {
            Debug.LogError("Terjadi kesalahan saat mengurangi HP musuh : " + e.Message); //nanti diganti pakai Notif (masih belum)
        }
    }

    public void Special()
    {
        if (!isMoving && playerStat.mana >= 3)
        {
            _animator.SetTrigger("isSpecialTurnBase");
            DecreaseEnemyHealth(5);
            playerStat.mana -= 3;
            StartCoroutine(DoneAttacking(2f));
        }
    }

    public void Ultimate()
    {
        if (!isMoving && playerStat.energy < 100)
        {
            _animator.SetTrigger("isUltimateTurnBase");
            DecreaseEnemyHealth(10);
            playerStat.energy = 0;
            StartCoroutine(DoneAttacking(6f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        _animator.SetBool("isMoving", false);
        _animator.SetFloat("xDirection", 0);
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponent<EnemyBattle>();
        }

        if (_battleManager.GetCurrentTurn() == EnumTurns.player)
        {
            GetPushedButton(actionPlayer);
        }
    }

    public void GetPushedButton(EnumActions enumAction)
    {
        try
        {
            switch (enumAction)
            {
                case EnumActions.attack:
                    Attack();
                    break;
                case EnumActions.special:
                    Special();
                    break;
                case EnumActions.ultimate:
                    Ultimate();
                    break;
                default:
                    Debug.Log("no case"); //nanti pake notif (masih belum)
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("ada yang salah saat mendapatkan pushed button : " + e.Message);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        enemy = null;
    }
}
