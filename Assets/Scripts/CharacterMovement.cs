using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;
    private float verticalVelocity;

    private bool wasGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // =========================
        // INPUT
        // =========================
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0f, v).normalized;

        Vector3 moveDir =
            Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * input;

        bool grounded = controller.isGrounded;

        // =========================
        // LANDING DETECTION
        // =========================
        if (!wasGrounded && grounded && verticalVelocity <= 0f)
        {
            animator.SetTrigger("Land");
            verticalVelocity = -2f;
        }

        wasGrounded = grounded;

        // =========================
        // JUMP
        // =========================
        if (grounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }

        // =========================
        // GRAVITY
        // =========================
        verticalVelocity += gravity * Time.deltaTime;

        // Keep player "stuck" to ground slightly
        if (grounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        // =========================
        // AIR STATE
        // =========================
        bool isInAir = !grounded;

        bool isJumping = isInAir && verticalVelocity > 0.1f;
        bool isFalling = isInAir && verticalVelocity < 0.1f;

        if (grounded)
        {
            animator.SetBool("IsJumping", false);
        }

        // =========================
        // MOVE CHARACTER
        // =========================
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        // =========================
        // MOVEMENT CHECK
        // =========================
        bool isMoving = input.magnitude > 0.1f;

        // =========================
        // ANIMATOR PARAMETERS
        // =========================
        animator.SetBool("IsGrounded", grounded);
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsFalling", isFalling);
        animator.SetBool("IsMoving", isMoving);

        // =========================
        // MOVE BLEND VALUES
        // =========================
        Vector3 localMove = transform.InverseTransformDirection(moveDir);

        animator.SetFloat("MoveX", localMove.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", localMove.z, 0.1f, Time.deltaTime);
    }
}