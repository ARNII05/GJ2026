using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControlls : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private const int moveSpeed = 20;

    private GameControls controls;

    private Animator animator;

    private GameObject visuals;

    public bool isMoving = false, isSleeping = false;

    private const float startSleepInterval = 10f;

    private float startSleeping = startSleepInterval;

    private float timeSleeping = 0f;

    private const float timeSleepingToLose = 5f;

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

        animator = GetComponentInChildren<Animator>();
        visuals = transform.Find("Visuals").gameObject;
    }

    private void Update()
    {
        moveInput = controls.Gameplay.Move.ReadValue<Vector2>().normalized;

        isMoving = moveInput.x != 0f;

        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            if (isSleeping)
            {
                isSleeping = false;
                animator.SetBool("IsSleeping", false);
            }

            startSleeping = startSleepInterval;
            timeSleeping = 0f;
            return;
        }

        startSleeping -= Time.deltaTime;

        if (startSleeping <= 0)
        {
            if (!isSleeping)
            {
                isSleeping = true;
                animator.SetBool("IsSleeping", true);
            }

            timeSleeping += Time.deltaTime;

            if (timeSleeping >= timeSleepingToLose)
            {
                GameManager.Instance.GameOver(false, true);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y
        );

        if (moveInput.x < 0)
            visuals.GetComponent<SpriteRenderer>().flipX = true;
        else if (moveInput.x > 0)
            visuals.GetComponent<SpriteRenderer>().flipX = false;
    }
}
