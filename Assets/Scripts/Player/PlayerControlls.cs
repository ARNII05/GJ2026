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
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        

        animator.SetBool("IsMoving", 0!=moveInput.x);



        if(moveInput.x == -1)

        {

            visuals.transform.localScale = new Vector3(-1, 1, 1);

        }else if(moveInput.x == 1)

        {

            visuals.transform.localScale = new Vector3(1, 1, 1);

        }
    }
}
