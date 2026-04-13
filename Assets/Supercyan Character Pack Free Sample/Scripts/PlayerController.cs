using UnityEngine; // Biblioteca principal do Unity

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidade de movimento do personagem
    public float jumpForce = 7f; // Força do pulo
    public Transform cameraTransform; // Referência à câmera para orientar o movimento

    private Rigidbody rb; // Componente Rigidbody do personagem (responsável pela física)
    private bool isGrounded; // Flag para saber se o personagem está no chão

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Pega o Rigidbody anexado ao personagem
    }

    void Update()
    {
        // Se apertar o botão de pulo e estiver no chão → chama Jump()
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move(); // Chamado a cada frame de física → controla movimento
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal"); // Entrada horizontal (A/D ou setas)
        float z = Input.GetAxis("Vertical");   // Entrada vertical (W/S ou setas)

        // Direção da câmera
        Vector3 camForward = cameraTransform.forward; // Frente da câmera
        Vector3 camRight = cameraTransform.right;     // Direita da câmera

        // Ignora inclinação da câmera (não deixa o personagem voar)
        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize(); // Normaliza vetor (magnitude = 1)
        camRight.Normalize();

        // Movimento baseado na câmera
        Vector3 move = camForward * z + camRight * x;

        // Define a velocidade do Rigidbody (atenção: o correto é "rb.velocity", não "rb.linearVelocity")
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);

        // Rotaciona o personagem na direção do movimento
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }

    void Jump()
    {
        // Zera a velocidade vertical antes de aplicar o pulo
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Aplica força para cima (pulo)
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Se estiver colidindo com objeto marcado como "Ground" → está no chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Se parar de colidir com "Ground" → não está mais no chão
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}