using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float runSpeed = 10f;
    public float gravity = -19.6f; // Un valor de gravedad algo más fuerte se siente mejor en platformers
    public float jumpForce = 8f;

    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;

        // Resetear velocidad en Y cuando toca el suelo firme
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movimiento de ejes
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : speed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        // Lógica de Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Calculamos la fuerza del salto de forma matemática limpia
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            
            // Si el jugador salta estando en una plataforma, desvinculamos el parent de inmediato
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;

        // Movimiento vertical (gravedad/salto)
        controller.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        rb.velocity = pushDir * 4f;
    }
}