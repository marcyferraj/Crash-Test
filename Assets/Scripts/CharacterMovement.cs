using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0f, v).normalized;

        // Camera-relative movement (uses camera direction ONLY)
        Vector3 moveDir = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * input;

        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // Convert to LOCAL space for blend tree
        Vector3 localMove = transform.InverseTransformDirection(moveDir);

        animator.SetFloat("MoveX", localMove.x, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveY", localMove.z, 0.1f, Time.deltaTime);
        animator.SetBool("IsMoving", input.magnitude > 0.1f);
    }
}