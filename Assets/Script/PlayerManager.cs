using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInput action;
    private Rigidbody2D rb;
    private Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        direction = value.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = direction * 10;
    }
}
