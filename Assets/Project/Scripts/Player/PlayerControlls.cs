using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControlls : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private const int moveSpeed = 20;

    private GameControls controls;

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
        rb.constraints = RigidbodyConstraints2D.FreezePositionY |
                 RigidbodyConstraints2D.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        moveInput = controls.Gameplay.Move.ReadValue<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
}
