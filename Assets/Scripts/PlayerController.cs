using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public Transform cameraTransform;

    [Header("Rotação com o mouse")]
    public float mouseSensitivity = 2f; // sensibilidade do mouse
    private float horizontalRotation = -90f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        Debug.Log("PLAYERCONTROLLER START INICIOU!");
        rb = GetComponent<Rigidbody>();
        QualitySettings.vSyncCount = 0; // desativa V Sync para permitir controle total do frame rate
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked; // trava o cursor na tela
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        horizontalRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 camForward = transform.forward; // agora usa a frente do player
        Vector3 camRight = transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * z + camRight * x;

        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}