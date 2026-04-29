using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float airControl = 8f;
    public Transform cameraTransform;
    public Animator animator;

    [Header("Jump & Gravity")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float terminalVelocity = 53f;

    [Header("Ground Check")]
    public float groundedOffset = 0.2f;
    public float groundedRadius = 0.3f;
    public LayerMask groundLayers;

    [Header("Timeouts")]
    public float jumpTimeout = 0.5f;

    [Header("Evade")]
    public float evadeDistance = 5f;
    public float evadeDuration = 0.2f;
    public float evadeCooldown = 1f;

    private CharacterController controller;

    private float verticalVelocity;
    private float jumpTimeoutDelta;

    private Vector3 horizontalVelocity;
    private Vector3 inputDir;

    private bool grounded;
    private bool wasGrounded;

    private float evadeTimer;
    private float evadeCooldownTimer;
    private bool isEvading;
    private Vector3 evadeDirection;

    private PlayerSounds playerSounds;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpTimeoutDelta = jumpTimeout;

        playerSounds = GetComponent<PlayerSounds>();
    }

    void Update()
    {
        GroundedCheck();

        HandleEvadeInput();

        JumpAndGravity();

        Move();

        UpdateAnimator();
    }


    // =========================
    // GROUND CHECK
    // =========================
    void GroundedCheck()
    {
        Vector3 spherePos = transform.position + Vector3.down * groundedOffset;

        grounded = Physics.CheckSphere(
            spherePos,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );

        animator.SetBool("IsGrounded", grounded);
    }

    // =========================
    // EVADE INPUT
    // =========================
    void HandleEvadeInput()
    {
        if (evadeCooldownTimer > 0f)
            evadeCooldownTimer -= Time.deltaTime;

        if (isEvading || evadeCooldownTimer > 0f)
            return;

        if (!grounded)
            return;

        float h = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (h < -0.1f)
            {
                StartEvade(-1);
            }
            else if (h > 0.1f)
            {
                StartEvade(1);
            }
        }
    }

    void StartEvade(int direction)
    {
        isEvading = true;
        evadeTimer = evadeDuration;
        evadeCooldownTimer = evadeCooldown;

        Vector3 right = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * Vector3.right;
        evadeDirection = right * direction;

        animator.SetTrigger(direction == -1 ? "EvadeLeft" : "EvadeRight");


        playerSounds?.PlayEvade();
    }

    // =========================
    // JUMP + GRAVITY
    // =========================
    void JumpAndGravity()
    {
        if (grounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (Input.GetButtonDown("Jump") && jumpTimeoutDelta <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                animator.SetTrigger("Jump");
                playerSounds?.PlayJump();
            }

            if (jumpTimeoutDelta >= 0f)
                jumpTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;
        }

        if (verticalVelocity < terminalVelocity)
            verticalVelocity += gravity * Time.deltaTime;

        if (!wasGrounded && grounded)
        {
            playerSounds?.PlayLand();
        }

        wasGrounded = grounded;
    }

    // =========================
    // MOVE
    // =========================
    void Move()
    {
        if (isEvading)
        {
            evadeTimer -= Time.deltaTime;

            float speed = evadeDistance / evadeDuration;
            Vector3 evadeVelocity = evadeDirection * speed;

            controller.Move(evadeVelocity * Time.deltaTime);

            if (evadeTimer <= 0f)
                isEvading = false;

            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        inputDir = new Vector3(h, 0f, v).normalized;

        Vector3 moveDir =
            Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDir;

        Vector3 targetHorizontal = moveDir * moveSpeed;

        if (grounded)
        {
            horizontalVelocity = targetHorizontal;
        }
        else
        {
            horizontalVelocity += (targetHorizontal - horizontalVelocity)
                * airControl * Time.deltaTime;
        }

        Vector3 velocity = horizontalVelocity;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        if (grounded && horizontalVelocity.magnitude > 0.1f)
        {
            playerSounds?.PlayFootstep();
        }
    }

    // =========================
    // ANIMATOR
    // =========================
    void UpdateAnimator()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        animator.SetBool("IsGrounded", grounded);

        Vector3 moveDir =
            Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDir;

        Vector3 localMove = transform.InverseTransformDirection(moveDir);

        animator.SetFloat("MoveX", localMove.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", localMove.z, 0.1f, Time.deltaTime);

        animator.SetBool("IsMoving", grounded && inputDir.magnitude > 0.1f);
    }

    // =========================
    // DEBUG
    // =========================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 spherePos = transform.position + Vector3.down * groundedOffset;

        Gizmos.DrawWireSphere(spherePos, groundedRadius);
    }
}