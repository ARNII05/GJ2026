using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControlls : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private const int moveSpeed = 10;

    private GameControls controls;

    //Add this line
    private Animator animator;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnDestroy()
    {
        controls.Dispose();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new GameControls();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;


        //Add this line
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        moveInput = controls.Gameplay.Move.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        //Add this line
        animator.SetBool("IsMoving", 0!=moveInput.x);
    }
}
