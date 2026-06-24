using System.Collections;
using UnityEngine;

public class PlayerBattleManager : MonoBehaviour, BattleActionInterface
{
    [SerializeField] private StatsSO playerStat;
    [SerializeField] private EnemyBattle enemy;
    [SerializeField] Animator _animator;
    
    private bool isMoving = false;
    private bool doneAttacking = false;
    private Rigidbody2D rb;
    private Vector3 posisiAwal;
    private float speed = 15;


    private void Start()
    {
        playerStat = ScriptableObject.Instantiate(playerStat);
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        posisiAwal = transform.position;
        TriggerMovement();
    }

    private void Update()
    {
        if (isMoving)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
        if (doneAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, posisiAwal, speed * Time.deltaTime);
        }
    }

    private IEnumerator WaitForSecond(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        doneAttacking = true;
    }

    public void TriggerMovement()
    {
        isMoving = true;
    }

    public void Attack()
    {
        if (!isMoving)
        {
            _animator.SetTrigger("isAttackTurnBase");
            StartCoroutine(WaitForSecond(2f));
        }
        //animation attack
        //ngurangin darah musuh
        //balik movement
        //end turn
    }

    public void Special()
    {
        //pindah movement
        //animation special
        //ngurangin darah musuh
        //balik movement
        //end turn
    }

    public void Ultimate()
    {
        //cek energy penuh atau nggk
        //kalau nggk penuh batal
        // kalau penuh pindah movement
        // animation special
        // depleted energy
        // balik movement
        // end turn
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = false;
        if(collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponent<EnemyBattle>();
        }

        Attack();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        enemy = null;
    }
}
