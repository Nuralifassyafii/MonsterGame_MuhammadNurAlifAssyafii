using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInput action;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float xDirection;
    private float yDirection;
    private bool isHoldWeapon;
    private Animator _animator;
    private GameObject detectedGameObject;
    private NPCManager talkedNPC = null;
    private int counterDialogue = 0;
    private string IS_ATTACKING = "isAttacking";
    private string IS_MOVING = "isMoving";

    [SerializeField] private StatsSO playerStats;
    [SerializeField] private DialogueUIManager _dialogueUIManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _dialogueUIManager = FindFirstObjectByType<DialogueUIManager>();
        playerStats = ScriptableObject.Instantiate(playerStats);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        direction = value.ReadValue<Vector2>();
        xDirection = direction.x;
        yDirection = direction.y;
        _animator.SetFloat("xDirection", xDirection);
        SetIdle(xDirection, yDirection);
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
        }
        talkedNPC.SetActiveNotif(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        try
        {
            counterDialogue = 0;
            talkedNPC.SetActiveNotif(false);
            detectedGameObject = null;
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
            _dialogueUIManager.StatusDialogueUI(false);
            //OpenDialogue(counterDialogue);
            //counterDialogue++;
        }
    }

    public void OnAttackOpenWorld(InputAction.CallbackContext cntx)
    {
        if (cntx.started)
        {
            _animator.SetBool(IS_ATTACKING,true);
        }
        else if(cntx.canceled)
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
