using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private float xDirection;
    private float yDirection;
    private bool isHoldWeapon;
    private Animator _animator;
    private GameObject detectedGameObject;
    private NPCManager talkedNPC = null;
    private EnemyBattle enemyNPC = null;
    private int counterDialogue = 0;
    private string IS_ATTACKING = "isAttacking";
    private string IS_MOVING = "isMoving";
    private bool permissionMoving = true;
    private BattleUIManager _battleUIManager;
    private AudioManager _audioManager;

    [SerializeField] private DialogueUIManager _dialogueUIManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioManager = FindFirstObjectByType<AudioManager>();
        _dialogueUIManager = FindFirstObjectByType<DialogueUIManager>();
        _battleUIManager = FindFirstObjectByType<BattleUIManager>();
    }

    public EnemyBattle GetInteractedEnemy()
    {
        return enemyNPC;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        if (permissionMoving)
        {
            direction = value.ReadValue<Vector2>();
        }
        else
        {
            direction = new Vector2(0, 0);
        }
        xDirection = direction.x;
        yDirection = direction.y;
        _animator.SetFloat("xDirection", xDirection);
        SetIdle(xDirection, yDirection);
    }

    public void SetPermissionMoving(bool canMove)
    {
        permissionMoving = canMove;
    }

    public void SetIdle(float xDirection, float yDirection)
    {
        if (xDirection != 0 || yDirection != 0)
        {
            _animator.SetBool(IS_MOVING, true);
        }
        else
        {
            _animator.SetBool(IS_MOVING, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        detectedGameObject = collision.gameObject;
        CheckNPCObject(detectedGameObject);
        CheckEnemyObject(detectedGameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovingGate movedGate = collision.gameObject.GetComponent<MovingGate>();
        movedGate.Moving();
    }

    public void CheckNPCObject(GameObject detectedGameObject)
    {
        if (detectedGameObject.GetComponent<NPCManager>() != null)
        {
            talkedNPC = detectedGameObject.GetComponent<NPCManager>();
            talkedNPC.SetActiveNotif(true);
        }
    }

    public void CheckEnemyObject(GameObject detectedGameObject)
    {
        if (detectedGameObject.GetComponent<EnemyBattle>() != null)
        {
            enemyNPC = detectedGameObject.GetComponent<EnemyBattle>();
            enemyNPC.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            _battleUIManager.SetEnemyBattleManager(enemyNPC);
            enemyNPC.SetEnemySprites(true);
        }
    }

    public void SetPlayerObject(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        try
        {
            counterDialogue = 0;
            if(talkedNPC != null)
            {
            talkedNPC.SetActiveNotif(false);
            }
            if(enemyNPC != null)
            {
            enemyNPC.SetEnemySprites(false);
            }
            detectedGameObject = null;
            enemyNPC = null;
            talkedNPC = null;
            _dialogueUIManager.StatusDialogueUI(false);
        }
        catch (Exception e)
        {
            Debug.Log("Mohon maaf ada kesalahan : " + e.Message);
        }
    }

    public void OnInteractNPC(InputAction.CallbackContext context)
    {
        try
        {
            if (context.started)
            {
                InteractNPC();
            }
        }
        catch
        {
            Debug.Log("anda tidak bicara dengan siapa - siapa"); //nanti diganti Ui (masih belum)
        }
    }

    public void InteractNPC()
    {
        if (counterDialogue < talkedNPC.GetDialogueLength() && talkedNPC != null) //harusnya bisa digabung (masih belum)
        {
            OpenDialogue(counterDialogue);
            counterDialogue++;
        }
        else
        {
            counterDialogue = 0;
            talkedNPC.CheckHasItem();
            _dialogueUIManager.StatusDialogueUI(false);
        }
    }

    public void OnAttackOpenWorld(InputAction.CallbackContext cntx)
    {
        if (cntx.started)
        {
            _animator.SetBool(IS_ATTACKING, true);
            if(enemyNPC != null)
            {
                _audioManager.StopAllAudio();
                _audioManager.PlayAudio("battle");
                _battleUIManager.StartBattle();
            }
        }
        else if (cntx.canceled)
        {
            _animator.SetBool(IS_ATTACKING, false);
        }
    }

    public void OpenDialogue(int index)
    {

        _dialogueUIManager.SetDialogueName(talkedNPC.GetName());
        _dialogueUIManager.SetDialogueText(talkedNPC.GetDialogue(index));
        _dialogueUIManager.SetSprite(talkedNPC.GetSpriteNPC());
        _dialogueUIManager.StatusDialogueUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = direction * 10;
    }
}
