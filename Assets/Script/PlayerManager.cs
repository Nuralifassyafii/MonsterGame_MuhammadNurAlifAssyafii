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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = direction * 10;
    }
}
