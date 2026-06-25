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
    private AudioManager _audioManager;


    private void Start()
    {
        playerStat = ScriptableObject.Instantiate(playerStat);
        _battleManager = FindFirstObjectByType<BattleUIManager>();
        _audioManager = FindFirstObjectByType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        posisiAwal = transform.localPosition;
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
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, posisiAwal, speed * Time.deltaTime);
            }
    }

    public void PlayPunch()
    {
        _audioManager.PlayAudio("punch");
    }

    public void PlayExplosion(int number)
    {
        _audioManager.PlayAudio("explosion" + number.ToString());
    }

    public void PlayFire()
    {
        _audioManager.PlayAudio("fire");
    }

    public void PlayFireCrack()
    {
        _audioManager.PlayAudio("fireCrack");
    }

    public void PlayHeal()
    {
        _audioManager.PlayAudio("heal");
    }

    public void StopAudio()
    {
        _audioManager.StopAllAudio();
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
            //show panel death (masih belum)
        }
    }

    public void HideAllEffect()
    {
        for (int i = 0;i < effectList.Count;i++)
        {
            effectList[i].objectEffect.SetActive(false);
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
        if (!isMoving && playerStat.mana >= 5)
        {
            _animator.SetTrigger("isSpecialTurnBase");
            playerStat.energy += 5;
        }
        else
        {
            Debug.Log("Mana Not Enough"); //biasa nanti pake notif (masih belum)
        }
        StartCoroutine(DoneAttacking(2f));
    }

    public void Ultimate()
    {
        if (!isMoving && playerStat.energy == playerStat.maxEnergy)
        {
            _animator.SetTrigger("isUltimateTurnBase");
            StartCoroutine(DoneAttacking(6f));
        }
    }

    public void Heal()
    {
        if (!isMoving && playerStat.healthItem > 0)
        {
            _animator.SetTrigger("isHeal");
            playerStat.hp += 5;
            if(playerStat.hp > playerStat.maxHp)
            {
                playerStat.hp = playerStat.maxHp;
            }
            playerStat.healthItem--;
            playerStat.energy++;
            StartCoroutine(DoneAttacking(2f));
        }
    }

    public void DecreaseMana(int amount)
    {
        playerStat.mana -= amount;
        if (playerStat.mana < 0)
        {
            playerStat.mana = 0;
        }
    }

    public void DecreaseEnergy()
    {
        playerStat.energy = 0;
    }

    public void AddMana(int amount)
    {
        playerStat.mana += amount;
        if(playerStat.mana > playerStat.maxMana)
        {
            playerStat.mana = playerStat.maxMana;
        }
    }

    public void AddEnergy(int amount)
    {
        playerStat.energy += amount;
        if(playerStat.energy > playerStat.maxEnergy)
        {
            playerStat.energy = playerStat.maxEnergy;
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
                case EnumActions.item:
                    Heal();
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
