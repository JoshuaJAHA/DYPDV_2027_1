using UnityEngine;

public class PlayerMovementAnim : MonoBehaviour
{
    public Transform cameraTransform;

    Animator anim;
    CharacterController controller;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = Vector3.zero;

        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * z + camRight * x;
        }

        float speedValue = moveDir.magnitude;

        if (speedValue < 0.1f)
        {
            speedValue = 0f;
        }

        // --- ACTIVAMOS EL MOVIMIENTO POR CÓDIGO ---
        controller.Move(moveDir * Time.deltaTime * 3f);

        anim.SetFloat("Speed", speedValue);

        if (speedValue > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("IsJumping", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("IsJumping", false);
        }
    }
}